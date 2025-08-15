using MediatR;
using Shared.Wrapper;
using Shared.Exceptions;
using Domain.Interfaces.UnitOfWorks;

namespace Application.Features.Ownership.Organizations.Queries.GetById;

public partial class GetOrganizationByIdQuery : IRequest<Result<OrganizationDto>>
{
    public Guid Id { get; set; }
}

internal class GetOrganizationByIdQueryHandler(IAppUnitOfWork unitOfWork)
    : IRequestHandler<GetOrganizationByIdQuery, Result<OrganizationDto>>
{
    public async Task<Result<OrganizationDto>> Handle(GetOrganizationByIdQuery command, CancellationToken cancellationToken)
    {
        var result = new Result<OrganizationDto>();

        try
        {
            var organization = await unitOfWork.OrganizationRepository.GetByIdAsync(command.Id, cancellationToken);

            if (organization == null)
            {
                result.NotFound($"Organization with ID '{command.Id}' was not found.");
                return result;
            }

            var organizationDto = organization.ToDto();

            result.AddValue(organizationDto);
            result.AddSuccessMessage($"Organization '{organization.Name}' has been successfully retrieved.");
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
