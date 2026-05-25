using Travel_Explorer.Application.DTOs.Flights.Bookings;

namespace Travel_Explorer.Application.Features.FlightBookings.Queries.GetFlightBookingById
{
    public record GetFlightBookingByIdQuery(int Id) : IRequest<FlightBookingDto>;
}
