#region Using Directives
using MediatR;
using Shared.Wrapper;
using Shared.Exceptions;
using Microsoft.AspNetCore.Identity;
using Domain.Entities.Security;
using Application.Features.Security.Identity.Enums;
#endregion

namespace Application.Features.Security.Identity.Commands.ManageRoles;

public sealed class ManageUserRolesCommand 
    : IRequest<Result<string>>
{
    public Guid UserId { get; set; }
    public List<string> Roles { get; set; } = [];
    public RoleAction Action { get; set; } // Assign, Change, Remove
}

internal sealed class ManageUserRolesCommandHandler(UserManager<ApplicationUser> userManager)
    : IRequestHandler<ManageUserRolesCommand, Result<string>>
{
    public async Task<Result<string>> Handle(ManageUserRolesCommand request,
        CancellationToken cancellationToken)
    {
        var result = new Result<string>();

        try
        {
            var user = await userManager.FindByIdAsync(request.UserId.ToString()) 
                ?? throw new NotFoundException("User not found.");
            var currentRoles = await userManager.GetRolesAsync(user);

            switch (request.Action)
            {
                case RoleAction.Assign:
                    var rolesToAdd = request.Roles.Except(currentRoles).ToList();
                    if (rolesToAdd.Count != 0)
                        await userManager.AddToRolesAsync(user, rolesToAdd);
                    result.AddSuccessMessage("Roles assigned successfully.");
                    break;

                case RoleAction.Change:
                    await userManager.RemoveFromRolesAsync(user, currentRoles);
                    await userManager.AddToRolesAsync(user, request.Roles);
                    result.AddSuccessMessage("Roles changed successfully.");
                    break;

                case RoleAction.Remove:
                    if (request.Roles.Count == 0)
                        await userManager.RemoveFromRolesAsync(user, currentRoles); // Remove all roles
                    else
                        await userManager.RemoveFromRolesAsync(user, request.Roles);
                    result.AddSuccessMessage("Roles removed successfully.");
                    break;
            }

            result.OK();
            result.AddValue($"User roles updated: {string.Join(", ", await userManager.GetRolesAsync(user))}");
        }
        catch (Exception ex)
        {
            throw new SqlDomainException("Error managing user roles.", new Dictionary<string, string[]>
            {
                { "InnerException", new[] { ex.InnerException?.ToString() ?? ex.Message } }
            });
        }

        return result;
    }
}
