using Travel_Explorer.Domain.Entities;

namespace Travel_Explorer.Domain.Interfaces
{
    public interface IFlightSchedualRepository
    {
        void Version(FlightSchedule flightBooking, string version);
    }
}
