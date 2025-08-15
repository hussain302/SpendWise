using Application.Services;
using Microsoft.AspNetCore.Mvc.Filters;
using Shared.Exceptions;
using System.IdentityModel.Tokens.Jwt;

namespace EndPoints.Infrastructure.Attributes;

[AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
public class CustomSpendWiseAuthorizeAttribute
    : Attribute, IAuthorizationFilter
{
    public void OnAuthorization(AuthorizationFilterContext context)
    {
        var token = context.HttpContext.Request.Headers.Authorization.FirstOrDefault()?.Split(" ").Last();
        var jwtService = context.HttpContext.RequestServices.GetRequiredService<IJwtService>();
        var unauthExceptionMessage = "Unauthorized access: Invalid or missing authentication token.";
        var forbiddenExceptionMessage = "Forbidden: You do not have permission to access this resource.";

        try
        {
            if (string.IsNullOrEmpty(token) || !jwtService.Validate(token)) throw new UnauthorizedException(error: unauthExceptionMessage);

            var handler = new JwtSecurityTokenHandler();

            if (handler.ReadToken(token) is not JwtSecurityToken jwtToken) throw new UnauthorizedException(error: unauthExceptionMessage);

            var userId = jwtToken.Claims.FirstOrDefault(c => c.Type == JwtRegisteredClaimNames.Sid)?.Value;
            var roles = jwtToken.Claims.Where(c => c.Type == "Roles").Select(c => c.Value).ToList();

            if (string.IsNullOrEmpty(userId) || roles.Count == 0) throw new ForbiddenException(error: forbiddenExceptionMessage);

        }
        catch
        {
            throw new UnauthorizedException(error: unauthExceptionMessage);
        }
    }
}
