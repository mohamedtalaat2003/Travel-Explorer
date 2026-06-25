using System.ComponentModel.DataAnnotations;

namespace Travel_Explorer.Domain.Entities
{
    /// <summary>
    /// Represents an available flight schedule/route that users can book.
    /// Contains airline info, departure/arrival details, pricing, and seat availability.
    /// </summary>
    public class FlightSchedule : BaseEntity
    {
        /// <summary>
        /// The name of the airline operating this flight (e.g., "EgyptAir", "Emirates").
        /// </summary>
        public string Airline { get; set; }

        /// <summary>
        /// The unique flight number assigned by the airline (e.g., "MS800", "EK231").
        /// </summary>
        public string FlightNumber { get; set; }

        /// <summary>
        /// The city from which the flight departs (e.g., "Cairo").
        /// </summary>
        public string DepartureCity { get; set; }

        /// <summary>
        /// The city where the flight arrives/lands (e.g., "Dubai").
        /// </summary>
        public string ArrivalCity { get; set; }

        /// <summary>
        /// The scheduled date and time of departure.
        /// </summary>
        public DateTime DepartureTime { get; set; }

        /// <summary>
        /// The scheduled date and time of arrival.
        /// </summary>
        public DateTime ArrivalTime { get; set; }

        /// <summary>
        /// Ticket price for Economy class seats.
        /// </summary>
        public decimal EconomyPrice { get; set; }

        /// <summary>
        /// Ticket price for Business class seats.
        /// </summary>
        public decimal BusinessPrice { get; set; }

        /// <summary>
        /// Ticket price for First Class seats.
        /// </summary>
        public decimal FirstClassPrice { get; set; }

        /// <summary>
        /// The number of economy seats still available for booking.
        /// </summary>
        public int AvailableEconomySeats { get; set; }

        /// <summary>
        /// The number of business class seats still available for booking.
        /// </summary>
        public int AvailableBusinessSeats { get; set; }

        /// <summary>
        /// The number of first class seats still available for booking.
        /// </summary>
        public int AvailableFirstClassSeats { get; set; }

        [Timestamp]
        public byte[] Version { get; set; }
        // ===== Navigation Properties =====

        /// <summary>
        /// All flight bookings made for this schedule.
        /// </summary>
        public ICollection<FlightBooking> Bookings { get; set; } = new List<FlightBooking>();
    }
}
