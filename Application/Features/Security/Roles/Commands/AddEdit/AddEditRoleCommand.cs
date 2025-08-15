using MediatR;
using Shared.Wrapper;
using Shared.Exceptions;
using Domain.Interfaces.UnitOfWorks;
using System.ComponentModel.DataAnnotations;

namespace Application.Features.Security.Roles.Commands.AddEdit;
public partial class AddEditRoleCommand : IRequest<Result<Guid>>
{
    public Guid? Id { get; set; }

    [Required]
    public string Name { get; set; }
    [Required]
    public string Description { get; set; }
}

internal sealed class AddEditRoleCommandHandler(IAppUnitOfWork unitOfWork)
    : IRequestHandler<AddEditRoleCommand, Result<Guid>>
{
    private readonly IAppUnitOfWork _unitOfWork = unitOfWork;

    public async Task<Result<Guid>> Handle(AddEditRoleCommand command, 
        CancellationToken cancellationToken)
    {
        var result = new Result<Guid>();

        try
        {
            //await _unitOfWork.BeginTransactionAsync(cancellationToken);

            if (command.Id.HasValue && command.Id != Guid.Empty)
            {
                var existingEntity = await _unitOfWork.RolesRepository.GetByIdAsync(command.Id.Value,
                    cancellationToken);

                if (existingEntity == null)
                {
                   // await _unitOfWork.RollbackTransactionAsync(cancellationToken);
                    result.NotFound($"Role with ID '{command.Id}' does not exist.");
                    return result;
                }

                if (existingEntity.Name != null && existingEntity.Name.Equals(command.Name,
                    StringComparison.OrdinalIgnoreCase) &&
                    existingEntity.Description == command.Description)
                {
                    //await _unitOfWork.RollbackTransactionAsync(cancellationToken);
                    result.Conflict("No changes detected. The role details remain the same.");
                    return result;
                }

                var roleExist = await _unitOfWork.RolesRepository
                    .GetByNameAsync(command.Name, cancellationToken);

                if (roleExist != null 
                    && roleExist.Id != Guid.Empty 
                    && roleExist.Id != command.Id)
                {
                  //  await _unitOfWork.RollbackTransactionAsync(cancellationToken);
                    result.Conflict($"Role name '{command.Name}' is already in use.");
                    return result;
                }

                existingEntity.Name = command.Name;
                existingEntity.Description = command.Description;
                await _unitOfWork.RolesRepository.UpdateAsync(existingEntity,
                    cancellationToken);
            }
            else
            {
                var roleExist = await _unitOfWork.RolesRepository
                    .GetByNameAsync(command.Name, cancellationToken);

                if (roleExist != null)
                {
                    //await _unitOfWork.RollbackTransactionAsync(cancellationToken);
                    result.Conflict($"Role name '{command.Name}' is already in use.");
                    return result;
                }

                var newEntity = command.ToEntity();
                await _unitOfWork.RolesRepository.AddAsync(newEntity, cancellationToken);
                command.Id = newEntity.Id;
            }

            var saveResult = await _unitOfWork.SaveChangesAsync(cancellationToken);

            if (!saveResult)
            {
                //await _unitOfWork.RollbackTransactionAsync(cancellationToken);
                result.NotImplemented("An error occurred while saving the role.");
                return result;
            }

            //await _unitOfWork.CommitTransactionAsync(cancellationToken);

            result.AddValue(command.Id ?? Guid.Empty);
            result.AddSuccessMessage($"Role '{command.Name}' has been successfully {(command.Id.HasValue ? "updated" : "created")}.");
            result.OK();
        }
        catch (Exception ex)
        {
            await _unitOfWork.RollbackTransactionAsync(cancellationToken);
            throw new SqlDomainException("An error occurred while processing the role.", new Dictionary<string, string[]>
            {
                { "InnerException", new[] { ex.InnerException?.ToString() ?? ex.Message } }
            });
        }

        return result;
    }
}