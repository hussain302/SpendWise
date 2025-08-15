using MediatR;
using Shared.Wrapper;
using Shared.Exceptions;
using Domain.Interfaces.UnitOfWorks;

namespace Application.Features.Security.Roles.Queries.GetAll;

public partial class GetAllRolesQuery
    : IRequest<Result<IReadOnlyList<RoleDto>>>
{
}

internal class GetAllRolesQueryHandler(IAppUnitOfWork unitOfWork)
    : IRequestHandler<GetAllRolesQuery, Result<IReadOnlyList<RoleDto>>>
{
    public async Task<Result<IReadOnlyList<RoleDto>>> Handle(GetAllRolesQuery command,
        CancellationToken cancellationToken)
    {
        var result = new Result<IReadOnlyList<RoleDto>>();

        try
        {
            var roles = await unitOfWork.RolesRepository.GetAllAsync(cancellationToken);

            if (roles.Count() < 1)
            {
                result.NotFound($"Roles not found.");
                return result;
            }

            var roleDto = roles.Select(x => x.ToDto()).ToList();
            result.AddValue(roleDto);
            result.AddSuccessMessage($"Roles has been successfully retrieved.");
            result.OK();
        }
        catch (Exception ex)
        {
            throw new SqlDomainException("An error occurred while fetching the roles.", new Dictionary<string, string[]>
            {
                { "InnerException", new[] { ex.InnerException?.ToString() ?? ex.Message } }
            });
        }

        return result;
    }
}
