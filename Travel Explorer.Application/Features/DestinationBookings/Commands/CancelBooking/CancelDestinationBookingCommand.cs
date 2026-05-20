namespace Travel_Explorer.Application.Features.DestinationBookings.Commands.CancelBooking
{
    public record CancelDestinationBookingCommand(int Id) : IRequest<bool>;
}
