namespace Travel_Explorer.Application.Features.FlightBookings.Commands.CancelFlightBooking
{
    public record CancelFlightBookingCommand(int Id) : IRequest<bool>;
}
