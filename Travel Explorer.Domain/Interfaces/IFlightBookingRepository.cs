using Travel_Explorer.Domain.Entities;

namespace Travel_Explorer.Domain.Interfaces
{
    public interface IFlightBookingRepository
    {
        void Version(FlightBooking flightBooking, string version);
    }
}
