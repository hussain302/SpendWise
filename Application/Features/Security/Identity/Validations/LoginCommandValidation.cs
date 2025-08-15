using FluentValidation;
using Application.Features.Security.Identity.Commands.Login;

namespace Application.Features.Security.Identity.Validations;

public class LoginCommandValidation : AbstractValidator<LoginCommand>
{
    public LoginCommandValidation()
    {
        // Email validation
        RuleFor(x => x.Email)
            .NotEmpty().WithMessage("Email is required.")
            .Matches(@"^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,}$")
            .WithMessage("Invalid email format.");

        // Password validation (Minimum 8 characters)
        RuleFor(x => x.Password)
            .NotEmpty().WithMessage("Password is required.");
    }
}
