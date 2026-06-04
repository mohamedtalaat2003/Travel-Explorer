using Travel_Explorer.Domain.Enums;
using System.Text.Json.Serialization;

namespace Travel_Explorer.Application.Features.DestinationBookings.Commands.UpdateBookingStatus
{
    
    
    
    public record UpdateBookingStatusCommand(
        BookingStatus Status) : IRequest<DestinationBookingDto?>
    {
        [JsonIgnore]
        public int Id { get; set; }
    }
}
