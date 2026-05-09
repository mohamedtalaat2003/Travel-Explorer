
namespace Travel_Explorer.Application.Features.DestinationBookings.Queries.GetMyBookings
{

    /// <summary>
    /// Returns all bookings for the currently authenticated traveler, optionally filtered by status.
    /// </summary>
    public record GetMyBookingsQuery(int UserId, string? Status = null)
        : IRequest<IReadOnlyList<DestinationBookingDto>>;

}
