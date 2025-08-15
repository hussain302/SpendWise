using Application.Features.Ownership.Organizations.Commands.AddEdit;
using Domain.Entities.Ownership;

namespace Application.Features.Ownership.Organizations;
public static class OrganizationMapper
{
    public static Domain.Entities.Ownership.Organization ToEntity(this AddEditOrganizationCommand source)
    => new()
    {
        Id = source.Id ?? Guid.Empty,
        Name = source.Name,
        Details = source.Details,
        HeadCount = source.HeadCount,
        OwnerName = source.OwnerName,
    };

    public static OrganizationDto ToDto(this Organization source)
    => new()
    {
        Id = source.Id,
        Name = source.Name,
        Details = source.Details,
        HeadCount = source.HeadCount,
        OwnerName = source.OwnerName,
        CreatedBy = source.CreatedBy,
        CreatedOn = source.CreatedOn,
        LastModifiedBy = source.LastModifiedBy,
        LastModifiedOn = source.LastModifiedOn
    };
}
