using FluentValidation;

namespace Travel_Explorer.Application.Features.ContactMessages.CreateContactMessage
{
    public class CreateContactMessageCommandValidator : AbstractValidator<CreateContactMessageCommand>
    {
        public CreateContactMessageCommandValidator()
        {
            RuleFor(x => x.FullName)
                .NotEmpty()
                .WithMessage("FullName is required.")
                .Length(3, 200)
                .WithMessage("FullName must be between 3 and 200 characters.");

            RuleFor(x => x.Email)
                .NotEmpty()
                .WithMessage("Email is required.")
                .EmailAddress()
                .WithMessage("Email must be a valid email address.")
                .MaximumLength(200)
                .WithMessage("Email must not exceed 200 characters.");

            RuleFor(x => x.Subject)
                .NotEmpty()
                .WithMessage("Subject is required.")
                .MaximumLength(300)
                .WithMessage("Subject must not exceed 300 characters.");

            RuleFor(x => x.Message)
                .NotEmpty()
                .WithMessage("Message is required.")
                .MaximumLength(2000)
                .WithMessage("Message must not exceed 2000 characters.");
        }
    }
}
