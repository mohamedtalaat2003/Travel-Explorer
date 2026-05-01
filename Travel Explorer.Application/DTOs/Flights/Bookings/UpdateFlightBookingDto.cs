using System.ComponentModel.DataAnnotations;
using Travel_Explorer.Domain.Enums;

namespace Travel_Explorer.Application.DTOs.Flights.Bookings
{
    public class UpdateFlightBookingDto
    {
        [Required]
        public int Id { get; set; }

        [Required]
        public BookingStatus Status { get; set; }
    }
}
