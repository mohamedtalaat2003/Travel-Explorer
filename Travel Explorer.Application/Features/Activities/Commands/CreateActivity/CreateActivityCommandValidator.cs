using FluentValidation;

namespace Travel_Explorer.Application.Features.Activities.Commands.CreateActivity
{
    public class CreateActivityCommandValidator : AbstractValidator<CreateActivityCommand>
    {
        public CreateActivityCommandValidator()
        {
            RuleFor(x => x.Name)
                .NotEmpty().WithMessage("Activity name is required.")
                .Length(2, 150).WithMessage("Activity name must be between 2 and 150 characters.");

            RuleFor(x => x.Description)
                .MaximumLength(1000).WithMessage("Description must not exceed 1000 characters.");

            RuleFor(x => x.Icon)
                .MaximumLength(200).WithMessage("Icon must not exceed 200 characters.");

            RuleFor(x => x.DestinationId)
                .GreaterThan(0).WithMessage("Destination ID is required and must be greater than 0.");
        }
    }
}
