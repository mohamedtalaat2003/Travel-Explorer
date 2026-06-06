
namespace Travel_Explorer.Application.Features.DestinationBookings.Queries.GetMyBookings
{

    
    
    
    public record GetMyBookingsQuery(string? Status = null)
        : IRequest<IReadOnlyList<DestinationBookingDto>>;

}
