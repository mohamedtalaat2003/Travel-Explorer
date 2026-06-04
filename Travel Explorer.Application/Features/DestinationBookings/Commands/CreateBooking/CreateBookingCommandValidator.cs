using FluentValidation;

namespace Travel_Explorer.Application.Features.DestinationBookings.Commands.CreateBooking
{
    public class CreateBookingCommandValidator : AbstractValidator<CreateBookingCommand>
    {
        public CreateBookingCommandValidator()
        {
            RuleFor(x => x.CheckInDate)
                .GreaterThanOrEqualTo(DateTime.UtcNow.Date.AddDays(-1))
                .WithMessage("CheckInDate must not be in the past.");

            RuleFor(x => x.CheckOutDate)
                .GreaterThan(x => x.CheckInDate)
                .WithMessage("CheckOutDate must be after CheckInDate.");

            RuleFor(x => x.NumberOfGuests)
                .InclusiveBetween(1, 100)
                .WithMessage("NumberOfGuests must be between 1 and 100.");

            RuleFor(x => x.DestinationId)
                .GreaterThan(0)
                .WithMessage("DestinationId must be greater than 0.");

            RuleFor(x => x.Notes)
                .MaximumLength(1000)
                .WithMessage("Notes must not exceed 1000 characters.");
            
        }
    }
}
