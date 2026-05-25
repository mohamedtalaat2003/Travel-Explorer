using Travel_Explorer.Application.DTOs.Flights.Bookings;
using Travel_Explorer.Domain.Enums;

namespace Travel_Explorer.Application.Features.FlightBookings.Commands.CreateFlightBooking
{
    public record CreateFlightBookingCommand(
        FlightClass Class,
        int NumberOfPassengers,
        int FlightScheduleId,
        string? SeatPreference,
        Gender Gender,
        string? Nationality) : IRequest<FlightBookingDto>;
}
