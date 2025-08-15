using MediatR;
using Shared.Wrapper;
using Shared.Exceptions;
using Domain.Interfaces.UnitOfWorks;
using System.ComponentModel.DataAnnotations;

namespace Application.Features.Ownership.Organizations.Commands.Delete;
public partial class DeleteOrganizationCommand : IRequest<Result<bool>>
{
    [Required]
    public Guid Id { get; set; }
}

internal sealed class DeleteOrganizationCommandCommandHandler(IAppUnitOfWork unitOfWork)
    : IRequestHandler<DeleteOrganizationCommand, Result<bool>>
{
    private readonly IAppUnitOfWork _unitOfWork = unitOfWork;

    public async Task<Result<bool>> Handle(DeleteOrganizationCommand command, CancellationToken cancellationToken)
    {
        var result = new Result<bool>();

        try
        {
            await _unitOfWork.BeginTransactionAsync(cancellationToken);

            if (command.Id != Guid.Empty)
            {
                var existingEntity = await _unitOfWork.OrganizationRepository.GetByIdAsync(command.Id, cancellationToken);

                if (existingEntity == null)
                {
                    await _unitOfWork.RollbackTransactionAsync(cancellationToken);
                    result.NotFound($"Organization with ID '{command.Id}' does not exist.");
                    return result;
                }

                await _unitOfWork.OrganizationRepository.DeleteAsync(existingEntity, cancellationToken);

                var saveResult = await _unitOfWork.SaveChangesAsync(cancellationToken);
                if (!saveResult)
                {
                    await _unitOfWork.RollbackTransactionAsync(cancellationToken);
                    result.NotImplemented("An error occurred while deleting the Organization.");
                    return result;
                }

                await _unitOfWork.CommitTransactionAsync(cancellationToken);

                result.AddValue(saveResult);
                result.AddSuccessMessage($"Organization '{existingEntity.Name}' has been successfully deleted.");
                result.OK();
                return result;
            }

            result.AddErrorMessage($"Organization Id: '{command.Id} is empty.");
            result.OK();
            result.IsSuccess = false;
            return result;
        }
        catch (Exception ex)
        {
            await _unitOfWork.RollbackTransactionAsync(cancellationToken);
            throw new SqlDomainException("An error occurred while processing the Organization.", new Dictionary<string, string[]>
            {
                { "InnerException", new[] { ex.InnerException?.ToString() ?? ex.Message } }
            });
        }
    }
}