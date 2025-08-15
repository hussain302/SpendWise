using Application.Features.Security.Roles.Commands.AddEdit;
using FluentValidation;

namespace Application.Features.Security.Roles.Validations;

public class AddEditRoleCommandValidation
    : AbstractValidator<AddEditRoleCommand>
{
    public AddEditRoleCommandValidation()
    {
        RuleFor(x => x.Name)
            .NotEmpty()
            .WithMessage("Must be a valid string");

        RuleFor(x => x.Name)
            .MaximumLength(20)
            .WithMessage("Maximum length of name is atleast 15 characters");

        RuleFor(x => x.Description)
            .MinimumLength(20)
            .WithMessage("Minumum length of name is atleast 20 characters");
    }
}
