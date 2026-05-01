using System.ComponentModel.DataAnnotations;

namespace Travel_Explorer.Application.DTOs.Flights.Schedules
{
    public class CreateFlightScheduleDto
    {
        [Required]
        [StringLength(100)]
        public string Airline { get; set; }

        [Required]
        [StringLength(20)]
        public string FlightNumber { get; set; }

        [Required]
        [StringLength(100)]
        public string DepartureCity { get; set; }

        [Required]
        [StringLength(100)]
        public string ArrivalCity { get; set; }

        [Required]
        public DateTime DepartureTime { get; set; }

        [Required]
        public DateTime ArrivalTime { get; set; }

        [Range(0, 1000000)]
        public decimal EconomyPrice { get; set; }

        [Range(0, 1000000)]
        public decimal BusinessPrice { get; set; }

        [Range(0, 1000000)]
        public decimal FirstClassPrice { get; set; }

        [Range(0, 1000)]
        public int AvailableSeats { get; set; }
    }
}
