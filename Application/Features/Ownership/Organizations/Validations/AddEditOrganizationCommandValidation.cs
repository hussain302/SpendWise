using FluentValidation;
using Application.Features.Ownership.Organizations.Commands.AddEdit;

namespace Application.Features.Security.Identity.Validations
{
    public class AddEditOrganizationCommandValidation : AbstractValidator<AddEditOrganizationCommand>
    {
        public AddEditOrganizationCommandValidation()
        {
            RuleFor(x => x.Name)
                .NotEmpty().WithMessage("Organization name is required.")
                .MinimumLength(3).WithMessage("Organization name must be at least 3 characters long.");

            RuleFor(x => x.OwnerName)
                .NotEmpty().WithMessage("Owner name is required.")
                .MinimumLength(3).WithMessage("Owner name must be at least 3 characters long.");

            RuleFor(x => x.Details)
                .MaximumLength(500).WithMessage("Details cannot exceed 500 characters.")
                .When(x => !string.IsNullOrEmpty(x.Details));

            RuleFor(x => x.HeadCount)
                .GreaterThanOrEqualTo(0).WithMessage("Head count cannot be negative.")
                .When(x => x.HeadCount.HasValue);
        }
    }
}
