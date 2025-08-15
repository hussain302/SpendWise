#region Using Directives
using MediatR;
using Shared.Wrapper;
using Shared.Exceptions;
using Microsoft.AspNetCore.Identity;
using Domain.Entities.Security;
using Microsoft.EntityFrameworkCore;
#endregion

namespace Application.Features.Security.Users.Queries.GetAll;

public sealed class GetAllUsersQuery : IRequest<Result<IReadOnlyList<UserDto>>>
{
    public Guid? UserId { get; set; }
    public bool? IsAdmin { get; set; }
    public bool? IsActive { get; set; }
}

internal sealed class GetAllUsersQueryHandler(UserManager<ApplicationUser> userManager)
    : IRequestHandler<GetAllUsersQuery, Result<IReadOnlyList<UserDto>>>
{
    public async Task<Result<IReadOnlyList<UserDto>>> Handle(GetAllUsersQuery request, CancellationToken cancellationToken)
    {
        var result = new Result<IReadOnlyList<UserDto>>();
        var userDtos = new List<UserDto>();

        var usersQuery = userManager.Users
            .Where(u => !request.UserId.HasValue || u.Id == request.UserId.Value)
            .Where(u => !request.IsActive.HasValue || u.IsActive == request.IsActive.Value);

        var users = await usersQuery.ToListAsync(cancellationToken);

        if (users.Count == 0)
            throw new NotFoundException("No user found.");

        foreach (var user in users)
        {
            var roles = await userManager.GetRolesAsync(user);
            if (request.IsAdmin.HasValue && request.IsAdmin.Value != roles.Contains("Admin"))
                continue;

            userDtos.Add(user.ToDto([.. roles]));
        }

        if (userDtos.Count == 0)
            throw new NotFoundException("No user match the criteria.");

        // Dynamically set success message
        var userMessage = userDtos.Count == 1 ? "User has been successfully retrieved."
                                              : "Users have been successfully retrieved.";

        result.OK();
        result.AddSuccessMessage(userMessage);
        result.AddValue(userDtos);
        return result;

    }
}
