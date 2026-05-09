using Travel_Explorer.Domain.Enums;

namespace Travel_Explorer.Application.DTOs.Flights.Bookings
{
    public class CreateFlightBookingDto
    {
        [Required]
        public FlightClass Class { get; set; }

        [Range(1, 10)]
        public int NumberOfPassengers { get; set; }

        [Required]
        public int FlightScheduleId { get; set; }

        [StringLength(100)]
        public string? SeatPreference { get; set; }
    }
}
