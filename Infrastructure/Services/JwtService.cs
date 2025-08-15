using System.Text;
using Application.Services;
using System.Security.Claims;
using Application.DTOs.Identity;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using Domain.Configurations;

namespace Infrastructure.Services;

public sealed class JwtService(IOptions<JwtOptions> jwtOptions) : IJwtService
{
    private readonly JwtOptions jwtOptions = jwtOptions.Value;

    public string GenerateToken(IdentityUserDto user)
    {
        // Initialize lists for permissions and roles
        var permissions = new List<string>();
        var roles = new List<string>();

        // Add roles from user
        if (user.Roles.Count > 0)
        {
            foreach (var role in user.Roles)
            {
                if (role != null)
                {
                    roles.Add(role);
                }
            }
        }

        // Add permissions based on roles
        // Uncomment and customize this part if permissions need to be retrieved per role
        /*
        if (user.Roles.Any(role => role.Permissions != null && role.Permissions.Any()))
        {
            foreach (var permission in user.Roles.SelectMany(role => role.Permissions))
            {
                if (permission != null)
                {
                    permissions.Add(permission.PermissionName);
                }
            }
        }
        */

        // Claims for roles and permissions
        var rolesClaims = roles.Select(role => new Claim("Roles", role)).ToArray();
        var permissionClaims = permissions.Select(permission => new Claim("Permissions", permission)).ToArray();

        // Basic claims
        var claims = new List<Claim>
        {
            new (JwtRegisteredClaimNames.Sid, user.UserId.ToString()),  // User ID as Sid (Subject Identifier)
            new (JwtRegisteredClaimNames.NameId, user.UserName),        // Username as NameId
            new (JwtRegisteredClaimNames.Email, user.Email),           // Email
            new ("IsActive", user.IsActive.ToString())                 // IsActive status
        };

        // Combine all claims
        claims.AddRange(rolesClaims);
        claims.AddRange(permissionClaims);

        // Create signing credentials with the security key
        var signingCredentials = new SigningCredentials(
            new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtOptions.SecurityKey)),
            SecurityAlgorithms.HmacSha256
        );

        // Create the JWT token with the claims and expiration time
        var token = new JwtSecurityToken(
            issuer: jwtOptions.Issuer,
            audience: jwtOptions.Audience,
            claims: claims,
            expires: DateTime.Now.AddMinutes(10),  // Adjust token expiration as needed
            signingCredentials: signingCredentials
        );

        // Write the token as a string
        string tokenValue = new JwtSecurityTokenHandler().WriteToken(token);

        return tokenValue;
    }

    public bool Validate(string token)
    {
        try
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.UTF8.GetBytes(jwtOptions.SecurityKey);

            var validationParameters = new TokenValidationParameters
            {
                ValidateIssuer = true,
                ValidIssuer = jwtOptions.Issuer,
                ValidateAudience = true,
                ValidAudience = jwtOptions.Audience,
                ValidateLifetime = true,
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(key),
                ClockSkew = TimeSpan.Zero
            };

            tokenHandler.ValidateToken(token, validationParameters, out _);
            return true;
        }
        catch
        {
            return false;
        }
    }
}
