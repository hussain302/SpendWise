using Domain.Contracts;

namespace Application.Features.Ownership.Organizations;
public class OrganizationDto : AuditableEntity<Guid>
{
    public string Name { get; init; }

    public string OwnerName { get; init; }

    public string? Details { get; init; }

    public int? HeadCount { get; init; }
}