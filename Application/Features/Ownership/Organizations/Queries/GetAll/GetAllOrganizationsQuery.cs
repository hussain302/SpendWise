using MediatR;
using Shared.Wrapper;
using Shared.Exceptions;
using Domain.Interfaces.UnitOfWorks;
using Application.Features.Security.Roles;
using Application.Features.Ownership.Organizations;

namespace Application.Features.Ownership.Organizations.Queries.GetAll;

public partial class GetAllOrganizationsQuery
    : IRequest<Result<IReadOnlyList<OrganizationDto>>>
{
}

internal class GetAllOrganizationsQueryHandler(IAppUnitOfWork unitOfWork)
    : IRequestHandler<GetAllOrganizationsQuery, Result<IReadOnlyList<OrganizationDto>>>
{
    public async Task<Result<IReadOnlyList<OrganizationDto>>> Handle(GetAllOrganizationsQuery command,
        CancellationToken cancellationToken)
    {
        var result = new Result<IReadOnlyList<OrganizationDto>>();

        try
        {
            var Organizations = await unitOfWork.OrganizationRepository.GetAllAsync(cancellationToken);

            if (Organizations.Count() < 1)
            {
                result.NotFound($"Organizations not found.");
                return result;
            }

            var OrganizationDto = Organizations.Select(x => x.ToDto()).ToList();
            result.AddValue(OrganizationDto);
            result.AddSuccessMessage($"Organizations has been successfully retrieved.");
            result.OK();
        }
        catch (Exception ex)
        {
            throw new SqlDomainException("An error occurred while fetching the Organizations.", new Dictionary<string, string[]>
            {
                { "InnerException", new[] { ex.InnerException?.ToString() ?? ex.Message } }
            });
        }

        return result;
    }
}
