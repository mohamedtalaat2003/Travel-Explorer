using Microsoft.EntityFrameworkCore;
using Travel_Explorer.Application.Common.Parameters;

namespace Travel_Explorer.Application.Features.Flights
{
    public class FlightScheduleSpecification : BaseSpecification<FlightSchedule>
    {
        public FlightScheduleSpecification(int id)
            : base(fs => fs.Id == id && !fs.IsDeleted)
        {
        }

        public FlightScheduleSpecification(string flightNumber, DateTime departureTime)
            : base(fs => fs.FlightNumber == flightNumber && fs.DepartureTime == departureTime)
        {
        }

        public FlightScheduleSpecification(FlightScheduleSpecParams p)
            : base()
        {
            if (!string.IsNullOrWhiteSpace(p.DepartureCity))
            {
                AddCriteria(fs => EF.Functions.ILike(fs.DepartureCity, $"%{p.DepartureCity}%"));
            }

            if (!string.IsNullOrWhiteSpace(p.ArrivalCity))
            {
                AddCriteria(fs => EF.Functions.ILike(fs.ArrivalCity, $"%{p.ArrivalCity}%"));
            }

            if (p.DepartureDate.HasValue)
            {
                var startDate = DateTime.SpecifyKind(p.DepartureDate.Value.Date, DateTimeKind.Utc);
                var endDate = DateTime.SpecifyKind(startDate.AddDays(1), DateTimeKind.Utc);
                AddCriteria(fs => fs.DepartureTime >= startDate && fs.DepartureTime < endDate);
            }

            AddOrderBy(fs => fs.DepartureTime);
            ApplyPaging((p.PageNumber - 1) * p.PageSize, p.PageSize);
        }
    }
}
