using Application.Features.Security.Roles.Commands.AddEdit;
using Domain.Entities.Security;

namespace Application.Features.Security.Roles;
public static class RoleMapper
{
    public static ApplicationRole ToEntity(this AddEditRoleCommand source)
    => new()
    {
        Id = source.Id ?? Guid.Empty,
        Name = source.Name,
        Description = source.Description,
    };
    
    public static RoleDto ToDto(this ApplicationRole source)
    => new()
    {
        Id = source.Id,
        Name = source.Name ?? string.Empty,
        Description = source.Description
    };
}
