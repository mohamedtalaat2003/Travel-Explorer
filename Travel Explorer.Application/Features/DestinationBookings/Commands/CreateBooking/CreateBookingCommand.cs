namespace Travel_Explorer.Application.Features.DestinationBookings.Commands.CreateBooking
{
    /// <summary>
    /// Creates a new destination booking.
    /// </summary>
    public record CreateBookingCommand(
        DateTime CheckInDate,
        DateTime CheckOutDate,
        int NumberOfGuests,
        int DestinationId,
        string? Notes) : IRequest<DestinationBookingDto>;
}
