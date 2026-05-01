using System.ComponentModel.DataAnnotations;

namespace Travel_Explorer.Application.DTOs.Bookings
{
    public class CreateDestinationBookingDto
    {
        [Required]
        public DateTime CheckInDate { get; set; }

        [Required]
        public DateTime CheckOutDate { get; set; }

        [Required]
        [Range(1, 100)]
        public int NumberOfGuests { get; set; }

        [Required]
        public int DestinationId { get; set; }

        [StringLength(1000)]
        public string? Notes { get; set; }
    }
}
