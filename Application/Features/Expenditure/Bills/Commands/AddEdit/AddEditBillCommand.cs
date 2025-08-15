#region Using Directives
using MediatR;
using Shared.Wrapper;
using Shared.Exceptions;
using Domain.Interfaces.UnitOfWorks;
using System.ComponentModel.DataAnnotations;
using Application.Services;
using Domain.Entities.Expenditure;
using Domain.Interfaces;
#endregion

namespace Application.Features.Expenditure.Bills.Commands.AddEdit;

public class AddEditBillCommand : IRequest<Result<Guid>>
{
    public Guid? Id { get; set; }

    [Required]
    [StringLength(50)]
    public string Title { get; set; }

    [Required]
    public decimal BillAmount { get; set; }

    [Required]
    [StringLength(int.MaxValue)]
    public string Description { get; set; }

    public Guid OrganizationId { get; set; }

    public Guid PaidById { get; set; }

    public List<SharedWithUser> SharedWithUser { get; set; }
}

public class SharedWithUser
{
    public Guid SharedWithUserId { get; set; }
    public bool PaidOnSpot { get; set; }
}

internal sealed class AddEditBillCommandHandler(IAppUnitOfWork unitOfWork,
    IDateTimeService dateTime,
    ICurrentUser currentUser) 
    : IRequestHandler<AddEditBillCommand, Result<Guid>>
{
    private readonly IAppUnitOfWork _unitOfWork = unitOfWork;
    private readonly IDateTimeService _dateTime = dateTime;
    private readonly ICurrentUser _currentUser = currentUser;

    public async Task<Result<Guid>> Handle(AddEditBillCommand command,
        CancellationToken cancellationToken)
    {
        var result = new Result<Guid>();
        var isCreateOrUpdateCommand = command.Id.HasValue;

        try
        {
            await _unitOfWork.BeginTransactionAsync(cancellationToken);

            if (command.Id.HasValue && command.Id != Guid.Empty)
            {
                var existingEntity = await _unitOfWork.BillsRepository
                    .GetByIdAsync(command.Id.Value, cancellationToken);

                if (existingEntity == null)
                {
                    result.NotFound($"Bill with ID '{command.Id}' does not exist.");
                    return result;
                }

                if (existingEntity.PaidById != Guid.Empty
                    && existingEntity.PaidById == command.PaidById
                    && existingEntity.Description == command.Description
                    && existingEntity.OrganizationId == command.OrganizationId
                    && existingEntity.BillAmount == command.BillAmount
                    && existingEntity.Title.Equals(command.Title, StringComparison.OrdinalIgnoreCase))
                {
                    result.Conflict("No changes detected. The Bill details remain the same.");
                    return result;
                }

                var billExist = await _unitOfWork.BillsRepository
                    .AnyAsync(x => x.Title == command.Title && x.Id != command.Id, cancellationToken);

                if (billExist)
                {
                    result.Conflict($"Bill title '{command.Title}' is already in use.");
                    return result;
                }

                existingEntity = command.ToEntity();
                existingEntity.BilledOn = _dateTime.DateTimeUtc;
                existingEntity.IsEmailSent = false;
                existingEntity.LastModifiedBy = _currentUser.UserName;

                // Update bill details
                await UpdateBillDetails(command, existingEntity, _dateTime.DateTimeUtc, cancellationToken);

                await _unitOfWork.BillsRepository.UpdateAsync(existingEntity, cancellationToken);
            }
            else
            {
                var billExist = await _unitOfWork.BillsRepository
                    .AnyAsync(x => x.Title == command.Title, cancellationToken);

                if (billExist)
                {
                    result.Conflict($"Bill name '{command.Title}' is already in use.");
                    return result;
                }

                var newEntity = command.ToEntity();
                newEntity.CreatedBy = _currentUser.UserName;
                newEntity.CreatedOn = _dateTime.DateTimeUtc;
                newEntity.BilledOn = _dateTime.DateTimeUtc;

                await _unitOfWork.BillsRepository.AddAsync(newEntity, cancellationToken);

                await AddBillDetails(command, newEntity, _dateTime.DateTimeUtc, cancellationToken);

                command.Id = newEntity.Id;
            }

            var saveResult = await _unitOfWork.SaveChangesAsync(cancellationToken);

            if (!saveResult)
            {
                result.NotImplemented("An error occurred while saving the Bill.");
                return result;
            }

            await _unitOfWork.CommitTransactionAsync(cancellationToken);

            result.AddValue(command.Id ?? Guid.Empty);
            result.AddSuccessMessage($"Bill '{command.Title}' has been successfully {(isCreateOrUpdateCommand ? "updated" : "created")}.");
            result.OK();
        }
        catch (Exception ex)
        {
            result.AddErrorMessage("An error occurred while processing the Bill.");
            throw new SqlDomainException("An error occurred while processing the Bill.", new Dictionary<string, string[]>
            {
                { "InnerException", new[] { ex.InnerException?.ToString() ?? ex.Message } }
            });
        }

        return result;
    }

    #region Helping methods
    private async Task AddBillDetails(AddEditBillCommand command,
        Bill bill, 
        DateTime currentDatetime,
        CancellationToken cancellationToken)
    {
        if (command.SharedWithUser != null && command.SharedWithUser.Count != 0)
        {
            var totalShareAmount = command.BillAmount / command.SharedWithUser.Count;
            var billDetails = command.SharedWithUser.Select(sharedUser => new BillDetail
            {
                BillId = bill.Id,
                SharedWithId = sharedUser.SharedWithUserId,
                ShareAmount = totalShareAmount,
                CreatedBy = bill.CreatedBy,
                CreatedOn = currentDatetime,
                PaidOnSpot = sharedUser.PaidOnSpot
            }).ToList();

            await _unitOfWork.BillDetailsRepository.AddRangeAsync(billDetails, cancellationToken);
        }
    }
    private async Task UpdateBillDetails(AddEditBillCommand command,
        Bill bill, 
        DateTime currentDatetime,
        CancellationToken cancellationToken)
    {
        var existingBillDetails = await _unitOfWork.BillDetailsRepository
            .GetAllAsync(predicate: default,
                         selector: x=>x.Where(x => x.BillId == bill.Id),
                         cancellationToken);

        await _unitOfWork.BillDetailsRepository.DeleteRangeAsync(existingBillDetails, cancellationToken);

        await AddBillDetails(command, bill, currentDatetime, cancellationToken);
    }
    #endregion
}
