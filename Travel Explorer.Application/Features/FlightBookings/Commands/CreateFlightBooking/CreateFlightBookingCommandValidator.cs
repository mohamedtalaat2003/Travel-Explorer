using FluentValidation;

namespace Travel_Explorer.Application.Features.FlightBookings.Commands.CreateFlightBooking
{
    public class CreateFlightBookingCommandValidator : AbstractValidator<CreateFlightBookingCommand>
    {
        public CreateFlightBookingCommandValidator()
        {
            RuleFor(x => x.Class)
                .IsInEnum()
                .WithMessage("A valid flight class is required.");

            RuleFor(x => x.NumberOfPassengers)
                .InclusiveBetween(1, 10)
                .WithMessage("NumberOfPassengers must be between 1 and 10.");

            RuleFor(x => x.FlightScheduleId)
                .GreaterThan(0)
                .WithMessage("FlightScheduleId must be greater than 0.");

            RuleFor(x => x.SeatPreference)
                .MaximumLength(100)
                .WithMessage("SeatPreference must not exceed 100 characters.");
        }
    }
}
