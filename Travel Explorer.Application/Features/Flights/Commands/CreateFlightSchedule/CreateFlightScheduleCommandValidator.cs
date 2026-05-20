using FluentValidation;

namespace Travel_Explorer.Application.Features.Flights.Commands.CreateFlightSchedule
{
    public class CreateFlightScheduleCommandValidator : AbstractValidator<CreateFlightScheduleCommand>
    {
        public CreateFlightScheduleCommandValidator()
        {
            RuleFor(x => x.Airline)
                .NotEmpty().WithMessage("Airline is required.")
                .MaximumLength(100).WithMessage("Airline must not exceed 100 characters.");

            RuleFor(x => x.FlightNumber)
                .NotEmpty().WithMessage("Flight number is required.")
                .MaximumLength(20).WithMessage("Flight number must not exceed 20 characters.");

            RuleFor(x => x.DepartureCity)
                .NotEmpty().WithMessage("Departure city is required.")
                .MaximumLength(100).WithMessage("Departure city must not exceed 100 characters.");

            RuleFor(x => x.ArrivalCity)
                .NotEmpty().WithMessage("Arrival city is required.")
                .MaximumLength(100).WithMessage("Arrival city must not exceed 100 characters.");

            RuleFor(x => x.DepartureTime)
                .NotEmpty().WithMessage("Departure time is required.");

            RuleFor(x => x.ArrivalTime)
                .NotEmpty().WithMessage("Arrival time is required.")
                .GreaterThan(x => x.DepartureTime).WithMessage("Arrival time must be after departure time.");

            RuleFor(x => x.EconomyPrice)
                .InclusiveBetween(0, 1000000).WithMessage("Economy price must be between 0 and 1,000,000.");

            RuleFor(x => x.BusinessPrice)
                .InclusiveBetween(0, 1000000).WithMessage("Business price must be between 0 and 1,000,000.");

            RuleFor(x => x.FirstClassPrice)
                .InclusiveBetween(0, 1000000).WithMessage("First class price must be between 0 and 1,000,000.");

            RuleFor(x => x.AvailableEconomySeats)
                .InclusiveBetween(0, 1000).WithMessage("Economy seats must be between 0 and 1000.");

            RuleFor(x => x.AvailableBusinessSeats)
                .InclusiveBetween(0, 1000).WithMessage("Business seats must be between 0 and 1000.");

            RuleFor(x => x.AvailableFirstClassSeats)
                .InclusiveBetween(0, 1000).WithMessage("First class seats must be between 0 and 1000.");
        }
    }
}
