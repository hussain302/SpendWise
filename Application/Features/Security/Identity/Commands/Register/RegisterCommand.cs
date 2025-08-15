#region Using Directives
using MediatR;
using Application.Services;
using Domain.Entities.Security;
using Microsoft.AspNetCore.Identity;
using Shared.Wrapper;
using Shared.Exceptions;
using Shared.Security;
using Microsoft.Extensions.Options;
using System.Linq;
using Domain.Interfaces.Repositories.Ownership;
#endregion

namespace Application.Features.Security.Identity.Commands.Register;

public class RegisterCommand : IRequest<Result<RegisterResponse>>
{
    public Guid OrganizationId { get; set; }
    public string Email { get; set; }
    public string Password { get; set; }
    public string UserName { get; set; }
    public bool IsSoloUser { get; set; }
    public List<Guid> Roles { get; set; }
}

public record RegisterResponse
{
    public Guid CreatedUserId { get; init; }
    public Guid OrganizationId { get; init; }
    public bool IsActive { get; init; }

    //public string Token { get; set; }
}

internal sealed class RegisterCommandHandler(UserManager<ApplicationUser> userManager,
    RoleManager<ApplicationRole> roleManager,
    IOrganizationRepository organizationRepository,
    IJwtService tokenService,
    IOptions<PasswordSecurityKeyModel> passwordOptions)
    : IRequestHandler<RegisterCommand, Result<RegisterResponse>>
{

    IJwtService _tokenService = tokenService;
    PasswordSecurityKeyModel passwordKeyValue = passwordOptions.Value;

    public async Task<Result<RegisterResponse>> Handle(RegisterCommand request,
    CancellationToken cancellationToken)
    {
        var result = new Result<RegisterResponse>();

        if (await userManager.FindByEmailAsync(request.Email) is not null)
            throw new ConflictException("A user with this email already exists.");
        if (!await organizationRepository.AnyAsync(x => x.Id == request.OrganizationId,
            cancellationToken)) 
            throw new NotFoundException("A user with this OrganizationId doesn't exists.");
        
        var newUser = new ApplicationUser
        {
            Email = request.Email,
            UserName = request.UserName,
            EmailConfirmed = false,
            IsActive = false,
            NormalizedEmail = request.Email.ToUpper(),
            NormalizedUserName = request.UserName.ToUpper(),
            IsSoloUser = request.IsSoloUser,
            OrganizationId = request.OrganizationId
        };

        var createUserResult = await userManager.CreateAsync(newUser, request.Password);

        if (!createUserResult.Succeeded) throw new CustomValidationException(createUserResult.Errors.Select(e => e.Description));
        
        if (request.Roles.Count != 0)
        {
            var roleNames = (await Task.WhenAll(
                request.Roles.Select(async roleId =>
                {
                    var role = await roleManager.FindByIdAsync(roleId.ToString());
                    return role?.Name ?? throw new CustomValidationException([$"Role with ID '{roleId}' not found."]);
                })
            )).ToList();

            var roleResult = await userManager.AddToRolesAsync(newUser, roleNames);
            if (!roleResult.Succeeded)
            {
                throw new CustomValidationException(roleResult.Errors.Select(e => e.Description));
            }
        }

        //var identityUser = new DTOs.Identity.IdentityUserDto
        //{
        //    Email = newUser.Email,
        //    IsActive = newUser.IsActive,
        //    UserId = newUser.Id,
        //    UserName = newUser.UserName ?? string.Empty,
        //    Roles = [.. (await userManager.GetRolesAsync(newUser))]
        //};

        //var token = _tokenService.GenerateToken(identityUser);

        var registerResponse = new RegisterResponse
        {
            CreatedUserId = newUser.Id,
            IsActive = newUser.IsActive,
            OrganizationId = (Guid)newUser.OrganizationId
            //Token = token
        };

        result.IsSuccess = true;
        result.AddValue(registerResponse);
        result.AddSuccessMessage("User has been registered successfully.");

        return result;
    }
}