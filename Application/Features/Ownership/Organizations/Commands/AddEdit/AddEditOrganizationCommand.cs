using MediatR;
using Shared.Wrapper;
using Shared.Exceptions;
using Domain.Interfaces.UnitOfWorks;
using System.ComponentModel.DataAnnotations;

namespace Application.Features.Ownership.Organizations.Commands.AddEdit;

public class AddEditOrganizationCommand : IRequest<Result<Guid>>
{
    public Guid? Id { get; set; }
    public string Name { get; set; }
    public string OwnerName { get; set; }
    public string? Details { get; set; }
    public int? HeadCount { get; set; }
}

internal sealed class AddEditOrganizationCommandHandler(IAppUnitOfWork unitOfWork)
    : IRequestHandler<AddEditOrganizationCommand, Result<Guid>>
{
    private readonly IAppUnitOfWork _unitOfWork = unitOfWork;

    public async Task<Result<Guid>> Handle(AddEditOrganizationCommand command,
        CancellationToken cancellationToken)
    {
        var result = new Result<Guid>();
        var IsCreateOrUpdateCommand = command.Id.HasValue;
        try
        {
            if (command.Id.HasValue && command.Id != Guid.Empty)
            {
                var existingEntity = await _unitOfWork.OrganizationRepository
                    .GetByIdAsync(command.Id.Value,
                    cancellationToken);

                if (existingEntity == null)
                {
                    result.NotFound($"Organization with ID '{command.Id}' does not exist.");
                    return result;
                }

                if (existingEntity.Name != null 
                    && existingEntity.Name.Equals(command.Name, StringComparison.OrdinalIgnoreCase) &&
                    existingEntity.Details == command.Details
                    && existingEntity.HeadCount == command.HeadCount && existingEntity.OwnerName != null
                    && existingEntity.OwnerName.Equals(command.OwnerName, StringComparison.OrdinalIgnoreCase))
                {
                    result.Conflict("No changes detected. The Organization details remain the same.");
                    return result;
                }

                var organizationExist = await _unitOfWork.OrganizationRepository
                    .AnyAsync(x => x.Name == command.Name && x.Id != command.Id,
                    cancellationToken);

                if (organizationExist)
                {
                    result.Conflict($"Organization name '{command.Name}' is already in use.");
                    return result;
                }

                existingEntity = command.ToEntity();

                await _unitOfWork.OrganizationRepository.UpdateAsync(existingEntity,
                    cancellationToken);
            }
            else
            {
                var organizationExist = await _unitOfWork.OrganizationRepository
                    .AnyAsync(x => x.Name == command.Name, cancellationToken);

                if (organizationExist)
                {
                    result.Conflict($"Organization name '{command.Name}' is already in use.");
                    return result;
                }

                var newEntity = command.ToEntity();
                await _unitOfWork.OrganizationRepository.AddAsync(newEntity,
                    cancellationToken);

                command.Id = newEntity.Id;
            }

            var saveResult = await _unitOfWork.SaveChangesAsync(cancellationToken);

            if (!saveResult)
            {
                result.NotImplemented("An error occurred while saving the Organization.");
                return result;
            }

            result.AddValue(command.Id ?? Guid.Empty);
            result.AddSuccessMessage($"Organization '{command.Name}' has been successfully {(IsCreateOrUpdateCommand ? "updated" : "created")}.");
            result.OK();
        }
        catch (Exception ex)
        {
            throw new SqlDomainException("An error occurred while processing the Organization.", new Dictionary<string, string[]>
            {
                { "InnerException", new[] { ex.InnerException?.ToString() ?? ex.Message } }
            });
        }

        return result;
    }
}