#region Using Directives
using MediatR;
using Domain.Entities.Security;
using Microsoft.AspNetCore.Identity;
using Shared.Wrapper;
using Shared.Exceptions;
#endregion

namespace Application.Features.Security.Users.Commands.Delete;

public class DeleteUserCommand : IRequest<Result<bool>>
{
    public Guid Id { get; set; }
}

internal sealed class DeleteUserCommandHandler(UserManager<ApplicationUser> userManager)
    : IRequestHandler<DeleteUserCommand, Result<bool>>
{

    public async Task<Result<bool>> Handle(DeleteUserCommand request,
        CancellationToken cancellationToken)
    {

        var result = new Result<bool>();
        var user = await userManager.FindByIdAsync(request.Id.ToString()) ?? throw new NotFoundException("User not found.");

        var deleteResult = await userManager.DeleteAsync(user);

        if (deleteResult.Errors.Any()) throw new CustomValidationException(deleteResult.Errors.Select(e => e.Description));

        result.OK();
        result.AddValue(deleteResult.Succeeded);
        result.IsSuccess = deleteResult.Succeeded;
        result.AddSuccessMessage(deleteResult.Succeeded ? "User delete successfully" : "Error while deleting user.");

        return result;
    }
}