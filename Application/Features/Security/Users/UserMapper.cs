using Domain.Entities.Security;

namespace Application.Features.Security.Users;
internal static class UserMapper
{
    public static UserDto ToDto(this ApplicationUser userSource,
        List<string> roleNames)
    => new()
    {
        Id = userSource.Id,
        Email = userSource.Email ?? string.Empty,
        IsActive = userSource.IsActive,
        IsEmailConfirmed = userSource.EmailConfirmed,
        UserName = userSource.UserName ?? string.Empty,
        UserRolesNames = roleNames
    };
}
