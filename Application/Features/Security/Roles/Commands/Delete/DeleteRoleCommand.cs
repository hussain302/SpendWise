using MediatR;
using Shared.Wrapper;
using Shared.Exceptions;
using Domain.Interfaces.UnitOfWorks;
using System.ComponentModel.DataAnnotations;

namespace Application.Features.Security.Roles.Commands.Delete;
public partial class DeleteRoleCommand : IRequest<Result<bool>>
{
    [Required]
    public Guid Id { get; set; }
}

internal sealed class DeleteRoleCommandCommandHandler(IAppUnitOfWork unitOfWork)
    : IRequestHandler<DeleteRoleCommand, Result<bool>>
{
    private readonly IAppUnitOfWork _unitOfWork = unitOfWork;

    public async Task<Result<bool>> Handle(DeleteRoleCommand command, CancellationToken cancellationToken)
    {
        var result = new Result<bool>();

        try
        {
            await _unitOfWork.BeginTransactionAsync(cancellationToken);

            if (command.Id != Guid.Empty)
            {
                var existingEntity = await _unitOfWork.RolesRepository.GetByIdAsync(command.Id, cancellationToken);

                if (existingEntity == null)
                {
                    await _unitOfWork.RollbackTransactionAsync(cancellationToken);
                    result.NotFound($"Role with ID '{command.Id}' does not exist.");
                    return result;
                }

                await _unitOfWork.RolesRepository.DeleteAsync(existingEntity.Id, cancellationToken);

                var saveResult = await _unitOfWork.SaveChangesAsync(cancellationToken);
                if (!saveResult)
                {
                    await _unitOfWork.RollbackTransactionAsync(cancellationToken);
                    result.NotImplemented("An error occurred while deleting the role.");
                    return result;
                }

                await _unitOfWork.CommitTransactionAsync(cancellationToken);

                result.AddValue(saveResult);
                result.AddSuccessMessage($"Role '{existingEntity.Name}' has been successfully deleted.");
                result.OK();
                return result;
            }

            result.AddErrorMessage($"Role Id: '{command.Id} is empty.");
            result.OK();
            result.IsSuccess = false;
            return result;
        }
        catch (Exception ex)
        {
            await _unitOfWork.RollbackTransactionAsync(cancellationToken);
            throw new SqlDomainException("An error occurred while processing the role.", new Dictionary<string, string[]>
            {
                { "InnerException", new[] { ex.InnerException?.ToString() ?? ex.Message } }
            });
        }
    }
}