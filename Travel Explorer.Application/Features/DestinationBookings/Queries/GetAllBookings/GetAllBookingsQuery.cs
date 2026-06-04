
namespace Travel_Explorer.Application.Features.DestinationBookings.Queries.GetAllBookings
{

    
    
    
    public record GetAllBookingsQuery(string? Status = null)
        : IRequest<IReadOnlyList<DestinationBookingDto>>;

}
