using System.Text.Json.Serialization;

namespace Travel_Explorer.Application.Features.DestinationBookings.Commands.UpdateBookingNotes
{
    
    
    
    public record UpdateBookingNotesCommand(
        string? Notes) : IRequest<DestinationBookingDto?>
    {
        [JsonIgnore]
        public int Id { get; set; }
    }
}
