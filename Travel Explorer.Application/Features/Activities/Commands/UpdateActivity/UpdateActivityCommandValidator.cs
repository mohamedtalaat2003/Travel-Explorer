using FluentValidation;

namespace Travel_Explorer.Application.Features.Activities.Commands.UpdateActivity
{
    public class UpdateActivityCommandValidator : AbstractValidator<UpdateActivityCommand>
    {
        public UpdateActivityCommandValidator()
        {
            RuleFor(x => x.Id)
                .GreaterThan(0).WithMessage("Activity ID is required and must be greater than 0.");

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
