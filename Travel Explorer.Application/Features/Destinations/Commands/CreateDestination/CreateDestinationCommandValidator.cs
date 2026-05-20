using FluentValidation;

namespace Travel_Explorer.Application.Features.Destinations.Commands.CreateDestination
{
    public class CreateDestinationCommandValidator : AbstractValidator<CreateDestinationCommand>
    {
        public CreateDestinationCommandValidator()
        {
            RuleFor(x => x.Name)
                .NotEmpty().WithMessage("Destination name is required.")
                .Length(3, 200).WithMessage("Destination name must be between 3 and 200 characters.");

            RuleFor(x => x.Description)
                .NotEmpty().WithMessage("Description is required.")
                .MaximumLength(2000).WithMessage("Description must not exceed 2000 characters.");

            RuleFor(x => x.Location)
                .NotEmpty().WithMessage("Location is required.")
                .MaximumLength(250).WithMessage("Location must not exceed 250 characters.");

            RuleFor(x => x.PricePerNight)
                .InclusiveBetween(0, 999999.99m).WithMessage("Price per night must be between 0 and 999999.99.");

            RuleFor(x => x.CategoryId)
                .GreaterThan(0).WithMessage("Category ID is required and must be greater than 0.");
        }
    }
}
