using MediatR;
using Shared.Wrapper;
using Shared.Exceptions;
using Domain.Interfaces.UnitOfWorks;
using System.ComponentModel.DataAnnotations;

namespace Application.Features.Expenditure.Bills.Commands.Delete;

public partial class DeleteBillCommand : IRequest<Result<bool>>
{
    [Required]
    public Guid Id { get; set; }
}

internal sealed class DeleteBillCommandHandler : IRequestHandler<DeleteBillCommand, Result<bool>>
{
    private readonly IAppUnitOfWork _unitOfWork;

    public DeleteBillCommandHandler(IAppUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<Result<bool>> Handle(DeleteBillCommand command, CancellationToken cancellationToken)
    {
        var result = new Result<bool>();

        try
        {
            // Begin a transaction to ensure consistency
            await _unitOfWork.BeginTransactionAsync(cancellationToken);

            if (command.Id != Guid.Empty)
            {
                var existingBill = await _unitOfWork.BillsRepository.GetByIdAsync(command.Id, cancellationToken);

                if (existingBill == null)
                {
                    // Rollback the transaction if the Bill doesn't exist
                    await _unitOfWork.RollbackTransactionAsync(cancellationToken);
                    result.NotFound($"Bill with ID '{command.Id}' does not exist.");
                    return result;
                }

                // Delete related BillDetails
                var existingBillDetails = await _unitOfWork.BillDetailsRepository
                    .GetAllAsync(
                        predicate: default,
                        selector: x => x.Where(x=>x.BillId == command.Id),
                        cancellationToken);

                if (existingBillDetails.Any())
                {
                    await _unitOfWork.BillDetailsRepository.DeleteRangeAsync(existingBillDetails, cancellationToken);
                }

                // Now, delete the Bill
                await _unitOfWork.BillsRepository.DeleteAsync(existingBill, cancellationToken);

                // Save changes and commit the transaction
                var saveResult = await _unitOfWork.SaveChangesAsync(cancellationToken);
                if (!saveResult)
                {
                    // Rollback the transaction if saving fails
                    await _unitOfWork.RollbackTransactionAsync(cancellationToken);
                    result.NotImplemented("An error occurred while deleting the Bill.");
                    return result;
                }

                // Commit transaction after successful delete operation
                await _unitOfWork.CommitTransactionAsync(cancellationToken);

                result.AddValue(saveResult);
                result.AddSuccessMessage($"Bill '{existingBill.Title}' has been successfully deleted.");
                result.OK();
                return result;
            }

            // Return an error if the Bill ID is empty
            result.AddErrorMessage($"Bill Id: '{command.Id}' is empty.");
            result.OK();
            result.IsSuccess = false;
            return result;
        }
        catch (Exception ex)
        {
            // Rollback transaction if an exception occurs
            await _unitOfWork.RollbackTransactionAsync(cancellationToken);
            throw new SqlDomainException("An error occurred while processing the Bill deletion.", new Dictionary<string, string[]>
            {
                { "InnerException", new[] { ex.InnerException?.ToString() ?? ex.Message } }
            });
        }
    }
}
