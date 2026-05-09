
namespace Travel_Explorer.Application.Features.DestinationBookings.Commands.DeleteBooking
{

    /// <summary>
    /// Soft-deletes a booking by ID.
    /// </summary>
    public record DeleteBookingCommand(int Id) : IRequest<bool>;

}
