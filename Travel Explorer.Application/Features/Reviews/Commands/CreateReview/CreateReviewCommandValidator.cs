using FluentValidation;

namespace Travel_Explorer.Application.Features.Reviews.Commands.CreateReview
{
    public class CreateReviewCommandValidator : AbstractValidator<CreateReviewCommand>
    {
        public CreateReviewCommandValidator()
        {
            RuleFor(x => x.Rating)
                .InclusiveBetween(1, 5)
                .WithMessage("Rating must be between 1 and 5.");

            RuleFor(x => x.Comment)
                .NotEmpty()
                .WithMessage("Comment is required.")
                .MaximumLength(1000)
                .WithMessage("Comment must not exceed 1000 characters.");

            RuleFor(x => x.DestinationId)
                .GreaterThan(0)
                .WithMessage("DestinationId must be greater than 0.");
        }
    }
}
