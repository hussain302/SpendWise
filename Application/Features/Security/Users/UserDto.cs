using Application.DTOs;
using Application.Features.Security.Roles;

namespace Application.Features.Security.Users;

public class UserDto : AutditableDto
{
    public Guid Id { get; set; }
    public string UserName { get; set; }
    public string Email { get; set; }
    public bool IsActive { get; set; }
    public bool IsEmailConfirmed { get; set; }
    public IEnumerable<RoleDto> UserRoles { get; set; }
    public IEnumerable<string> UserRolesNames { get; set; }
}