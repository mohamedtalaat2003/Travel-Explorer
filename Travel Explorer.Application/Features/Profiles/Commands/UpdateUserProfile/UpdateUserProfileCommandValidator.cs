using FluentValidation;

namespace Travel_Explorer.Application.Features.Profiles.Commands.UpdateUserProfile
{
    public class UpdateUserProfileCommandValidator : AbstractValidator<UpdateUserProfileCommand>
    {
        public UpdateUserProfileCommandValidator()
        {
            RuleFor(x => x.FullName)
                .NotEmpty().WithMessage("Full name is required.")
                .MaximumLength(150).WithMessage("Full name must not exceed 150 characters.");

            RuleFor(x => x.Email)
                .NotEmpty().WithMessage("Email address is required.")
                .EmailAddress().WithMessage("A valid email address is required.");

            RuleFor(x => x.PhoneNumber)
                .NotEmpty().WithMessage("Phone number is required.")
                .MaximumLength(20).WithMessage("Phone number must not exceed 20 characters.");

            RuleFor(x => x.PassportNumber)
                .NotEmpty().WithMessage("Passport number is required.")
                .MaximumLength(50).WithMessage("Passport number must not exceed 50 characters.");

            RuleFor(x => x.Bio)
                .MaximumLength(1000).WithMessage("Bio must not exceed 1000 characters.");

            RuleFor(x => x.Country)
                .MaximumLength(100).WithMessage("Country must not exceed 100 characters.");
        }
    }
}
