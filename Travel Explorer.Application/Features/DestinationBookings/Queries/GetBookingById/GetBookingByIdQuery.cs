
namespace Travel_Explorer.Application.Features.DestinationBookings.Queries.GetBookingById
{

    /// <summary>
    /// Returns a single booking by its ID.
    /// Non-admin users can only retrieve their own bookings.
    /// </summary>
    public record GetBookingByIdQuery(int Id) : IRequest<DestinationBookingDto?>;

}
