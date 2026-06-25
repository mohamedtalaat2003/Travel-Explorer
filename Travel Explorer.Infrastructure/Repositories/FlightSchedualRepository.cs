using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Travel_Explorer.Application.Features.FlightBookings.Commands.CreateFlightBooking;
using Travel_Explorer.Infrastructure.Data;

namespace Travel_Explorer.Infrastructure.Repositories
{
    public class FlightSchedualRepository(ApplicationDbContext _db) : IFlightSchedualRepository
    {
        public void Version(FlightSchedule flightSchedule, string version)
        {
            _db.Entry(flightSchedule).Property(x => x.Version).OriginalValue = Convert.FromBase64String(version);
        }
    }
}
