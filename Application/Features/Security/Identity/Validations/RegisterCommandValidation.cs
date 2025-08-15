using FluentValidation;
using Application.Features.Security.Identity.Commands.Register;

namespace Application.Features.Security.Identity.Validations;

public class RegisterCommandValidation : AbstractValidator<RegisterCommand>
{
    public RegisterCommandValidation()
    {
        RuleFor(x => x.Email)
         .NotEmpty().WithMessage("Email is required.")
         .Matches(@"^[^@\s]+@[^@\s]+\.[^@\s]+$").WithMessage("Invalid email format.");

        RuleFor(x => x.Password)
            .NotEmpty().WithMessage("Password is required.")
            .MinimumLength(8).WithMessage("Password must be at least 8 characters long.")
            .Matches("[A-Z]").WithMessage("Password must contain at least one uppercase letter.")
            .Matches("[a-z]").WithMessage("Password must contain at least one lowercase letter.")
            .Matches("[0-9]").WithMessage("Password must contain at least one number.")
            .Matches(@"[\W]").WithMessage("Password must contain at least one special character.");

        RuleFor(x => x.UserName)
            .NotEmpty().WithMessage("Username is required.")
            .Matches(@"^[a-zA-Z0-9_]+$").WithMessage("Username can only contain letters, numbers, and underscores.")
            .MaximumLength(50).WithMessage("Username cannot exceed 50 characters.");

        RuleFor(x => x.OrganizationId)
            .NotEmpty().WithMessage("OrganizationId is required.")
            .NotEqual(Guid.Empty).WithMessage("Invalid OrganizationId.");

        RuleFor(x => x.Roles)
            .NotNull().WithMessage("Roles are required.")
            .Must(roles => roles != null && roles.Count > 0).WithMessage("At least one role must be selected.");

        RuleFor(x => x.IsSoloUser)
            .NotNull().WithMessage("IsSoloUser field is required.");
    }
}
