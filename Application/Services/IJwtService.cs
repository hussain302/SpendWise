using Application.DTOs.Identity;
using Domain.Entities.Security;

namespace Application.Services;
public interface IJwtService
{
    string GenerateToken(IdentityUserDto user);
    bool Validate(string token);

}
