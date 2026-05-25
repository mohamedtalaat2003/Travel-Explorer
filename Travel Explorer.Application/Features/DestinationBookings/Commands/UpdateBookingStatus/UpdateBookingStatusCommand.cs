using Travel_Explorer.Domain.Enums;
using System.Text.Json.Serialization;

namespace Travel_Explorer.Application.Features.DestinationBookings.Commands.UpdateBookingStatus
{
    /// <summary>
    /// Updates the status of a booking.
    /// </summary>
    public record UpdateBookingStatusCommand(
        BookingStatus Status) : IRequest<DestinationBookingDto?>
    {
        [JsonIgnore]
        public int Id { get; set; }
    }
}
