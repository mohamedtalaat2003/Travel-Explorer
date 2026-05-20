using Travel_Explorer.Domain.Enums;

namespace Travel_Explorer.Application.DTOs.Flights.Bookings
{
    public class CreateFlightBookingDto
    {
        public FlightClass Class { get; set; }

        public int NumberOfPassengers { get; set; }

        public int FlightScheduleId { get; set; }

        public string? SeatPreference { get; set; }
    }
}
