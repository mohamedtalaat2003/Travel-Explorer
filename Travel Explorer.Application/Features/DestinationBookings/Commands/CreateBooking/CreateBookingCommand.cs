using System.Text.Json.Serialization;

namespace Travel_Explorer.Application.Features.DestinationBookings.Commands.CreateBooking
{
    /// <summary>
    /// Creates a new destination booking.
    /// </summary>
    public record CreateBookingCommand(
        [Required]
        DateTime CheckInDate,

        [Required]
        DateTime CheckOutDate,

        [Required]
        [Range(1, 100)]
        int NumberOfGuests,

        [Required]
        int DestinationId,

        [StringLength(1000)]
        string? Notes) : IRequest<DestinationBookingDto>
    {
        /// <summary>
        /// The ID of the traveler making the booking. Set by the controller.
        /// </summary>
        [JsonIgnore]
        public int UserId { get; set; }
    }
}
