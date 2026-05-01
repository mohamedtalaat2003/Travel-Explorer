using Travel_Explorer.Domain.Enums;

namespace Travel_Explorer.Application.DTOs.Flights.Bookings
{
    public class FlightBookingDto
    {
        public int Id { get; set; }
        public FlightClass Class { get; set; }
        public int NumberOfPassengers { get; set; }
        public decimal TotalPrice { get; set; }
        public BookingStatus Status { get; set; }
        public string? SeatPreference { get; set; }
        public int UserId { get; set; }
        public string UserFullName { get; set; }
        public int FlightScheduleId { get; set; }
        public string Airline { get; set; }
        public string FlightNumber { get; set; }
        public DateTime DepartureTime { get; set; }
    }
}
