using Travel_Explorer.Application.DTOs.Flights.Bookings;
using Travel_Explorer.Domain.Enums;

namespace Travel_Explorer.Application.Features.FlightBookings.Commands.UpdateFlightBookingStatus
{
    public record UpdateFlightBookingStatusCommand(int Id, BookingStatus Status) : IRequest<FlightBookingDto>;
}
