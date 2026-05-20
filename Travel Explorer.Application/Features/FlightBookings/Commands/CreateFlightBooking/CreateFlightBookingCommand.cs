using Travel_Explorer.Application.DTOs.Flights.Bookings;

namespace Travel_Explorer.Application.Features.FlightBookings.Commands.CreateFlightBooking
{
    public record CreateFlightBookingCommand(CreateFlightBookingDto Dto) : IRequest<FlightBookingDto>;
}
