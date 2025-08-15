#region Using Directives
using MediatR;
using Domain.Entities.Security;
using Microsoft.AspNetCore.Identity;
using Shared.Wrapper;
using Shared.Exceptions;
using System.ComponentModel.DataAnnotations;
#endregion

namespace Application.Features.Security.Identity.Commands.ConfirmEmail;

public sealed class ConfirmEmailCommand : IRequest<Result<bool>>
{
    [Required]
    public Guid Id { get; set; }
}

internal sealed class ConfirmUserCommandHandler(UserManager<ApplicationUser> userManager)
    : IRequestHandler<ConfirmEmailCommand, Result<bool>>
{
    public async Task<Result<bool>> Handle(ConfirmEmailCommand request,
        CancellationToken cancellationToken)
    {

        var result = new Result<bool>();
        var user = await userManager.FindByIdAsync(request.Id.ToString()) ?? throw new NotFoundException("User not found.");
        user.EmailConfirmed = true;
        var updateResult = await userManager.UpdateAsync(user);

        if (updateResult.Errors.Any()) throw new CustomValidationException(updateResult.Errors.Select(e => e.Description));

        result.OK();
        result.AddValue(updateResult.Succeeded);
        result.IsSuccess = updateResult.Succeeded;
        result.AddSuccessMessage(updateResult.Succeeded ? "User email confirmed." : "Error while confirming user.");

        return result;
    }
}