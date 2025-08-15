using MediatR;
using Shared.Wrapper;
using Shared.Exceptions;
using Domain.Interfaces.UnitOfWorks;

namespace Application.Features.Security.Roles.Queries.GetById;

public partial class GetRoleByIdQuery : IRequest<Result<RoleDto>>
{
    public Guid Id { get; set; }
}

internal class GetRoleByIdQueryHandler(IAppUnitOfWork unitOfWork) 
    : IRequestHandler<GetRoleByIdQuery, Result<RoleDto>>
{
    public async Task<Result<RoleDto>> Handle(GetRoleByIdQuery command, CancellationToken cancellationToken)
    {
        var result = new Result<RoleDto>();

        try
        {
            var role = await unitOfWork.RolesRepository.GetByIdAsync(command.Id, cancellationToken);

            if (role == null)
            {
                result.NotFound($"Role with ID '{command.Id}' was not found.");
                return result;
            }

            var roleDto = role.ToDto();

            result.AddValue(roleDto);
            result.AddSuccessMessage($"Role '{role.Name}' has been successfully retrieved.");
            result.OK();
        }
        catch (Exception ex)
        {
            throw new SqlDomainException("An error occurred while processing the role.", new Dictionary<string, string[]>
            {
                { "InnerException", new[] { ex.InnerException?.ToString() ?? ex.Message } }
            });
        }

        return result;
    }
}
