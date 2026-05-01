using System.ComponentModel.DataAnnotations;
using Travel_Explorer.Domain.Enums;

namespace Travel_Explorer.Application.DTOs.Bookings
{
    public class UpdateDestinationBookingDto
    {
        [Required]
        public int Id { get; set; }

        public BookingStatus Status { get; set; }

        [StringLength(1000)]
        public string? Notes { get; set; }
    }
}
