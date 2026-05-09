
namespace Travel_Explorer.Application.Features.DestinationBookings.Queries.GetBookingById
{

    /// <summary>
    /// Returns a single booking by its ID.
    /// </summary>
    public record GetBookingByIdQuery(int Id) : IRequest<DestinationBookingDto?>;

}
