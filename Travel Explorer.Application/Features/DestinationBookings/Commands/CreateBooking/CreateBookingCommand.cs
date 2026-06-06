namespace Travel_Explorer.Application.Features.DestinationBookings.Commands.CreateBooking
{
    
    
    
    public record CreateBookingCommand(
        DateTime CheckInDate,
        DateTime CheckOutDate,
        int NumberOfGuests,
        int DestinationId,
        string? Notes) : IRequest<DestinationBookingDto>;
}
