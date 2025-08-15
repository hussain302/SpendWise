#region Using Directives
using MediatR;
using Application.Services;
using Domain.Entities.Security;
using Microsoft.AspNetCore.Identity;
using Shared.Wrapper;
using Shared.Exceptions;
using Shared.Security;
using Microsoft.Extensions.Options;
#endregion

namespace Application.Features.Security.Identity.Commands.Login;

public class LoginCommand : IRequest<Result<LoginResponse>>
{
    public string Email { get; set; }
    public string Password { get; set; }
}

public class LoginResponse
{
    public string Token { get; set; }
}

internal sealed class LoginCommandHandler(UserManager<ApplicationUser> userManager, 
    IOptions<PasswordSecurityKeyModel> passwordOptions,
    IJwtService tokenService) 
    : IRequestHandler<LoginCommand, Result<LoginResponse>>
{

    PasswordSecurityKeyModel passwordKeyValue = passwordOptions.Value;

    public async Task<Result<LoginResponse>> Handle(LoginCommand request,
        CancellationToken cancellationToken)
    {

        var result = new Result<LoginResponse>();

        var user = await userManager.FindByEmailAsync(request.Email) ?? throw new NotFoundException("User not found.");
        if (!user.EmailConfirmed) throw new UnauthorizedException("Email is not confirmed. Please verify your email before logging in.");
        if (!user.IsActive) throw new UnauthorizedException("Your account is inactive. Please contact support for assistance.");
        if (!await userManager.CheckPasswordAsync(user, request.Password)) throw new UnauthorizedException("Incorrect password. Please try again.");
        
        var identityUser = new DTOs.Identity.IdentityUserDto
        {
            Email = request.Email,
            IsActive = user.IsActive,
            UserId = user.Id,
            UserName = user.UserName ?? string.Empty,
            Roles = []
        };

        var roles = await userManager.GetRolesAsync(user);

        identityUser.Roles.AddRange(roles);

        var token = tokenService.GenerateToken(identityUser);

        result.OK();
        result.AddValue(new LoginResponse { Token = token });
        result.AddSuccessMessage("Login Successful!");
       
        return result;
    }
}