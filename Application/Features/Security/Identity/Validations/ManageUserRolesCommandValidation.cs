using FluentValidation;
using Application.Features.Security.Identity.Commands.ManageRoles;

namespace Application.Features.Security.Identity.Validations
{
    public class ManageUserRolesCommandValidation : AbstractValidator<ManageUserRolesCommand>
    {
        public ManageUserRolesCommandValidation()
        {
            // UserId validation
            RuleFor(x => x.UserId)
                .NotEmpty().WithMessage("UserId is required.")
                .NotEqual(Guid.Empty).WithMessage("Invalid UserId.");

            // Roles validation (At least one role must be provided)
            RuleFor(x => x.Roles)
                .NotNull().WithMessage("Roles list cannot be null.")
                .Must(roles => roles != null && roles.Count > 0)
                .WithMessage("At least one role must be provided.");

            // Action validation
            RuleFor(x => x.Action)
                .IsInEnum().WithMessage("Invalid role action.");
        }
    }
}
