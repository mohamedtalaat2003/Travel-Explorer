
namespace Travel_Explorer.Application.Features.DestinationBookings.Queries.GetBookingById
{

    
    
    
    
    public record GetBookingByIdQuery(int Id) : IRequest<DestinationBookingDto?>;

}
