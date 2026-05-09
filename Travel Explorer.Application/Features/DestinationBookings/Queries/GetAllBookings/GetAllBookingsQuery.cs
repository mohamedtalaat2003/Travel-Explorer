
namespace Travel_Explorer.Application.Features.DestinationBookings.Queries.GetAllBookings
{

    /// <summary>
    /// Returns all bookings in the system (Admin only), optionally filtered by status.
    /// </summary>
    public record GetAllBookingsQuery(string? Status = null)
        : IRequest<IReadOnlyList<DestinationBookingDto>>;

}
