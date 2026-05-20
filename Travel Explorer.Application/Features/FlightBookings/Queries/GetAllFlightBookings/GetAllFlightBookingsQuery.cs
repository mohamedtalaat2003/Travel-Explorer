using Travel_Explorer.Application.Common;
using Travel_Explorer.Application.Common.Parameters;
using Travel_Explorer.Application.DTOs.Flights.Bookings;

namespace Travel_Explorer.Application.Features.FlightBookings.Queries.GetAllFlightBookings
{
    public record GetAllFlightBookingsQuery(FlightBookingSpecParams Params) : IRequest<PaginatedResult<FlightBookingDto>>;
}
