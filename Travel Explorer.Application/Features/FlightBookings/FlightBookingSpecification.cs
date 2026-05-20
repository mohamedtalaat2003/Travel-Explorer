using Travel_Explorer.Application.Common.Parameters;

namespace Travel_Explorer.Application.Features.FlightBookings
{
    public class FlightBookingSpecification : BaseSpecification<FlightBooking>
    {
        public FlightBookingSpecification(int id)
            : base(fb => fb.Id == id)
        {
            AddInclude(fb => fb.User);
            AddInclude(fb => fb.FlightSchedule);
        }

        public FlightBookingSpecification(FlightBookingSpecParams p, int? userId = null)
            : base()
        {
            if (userId.HasValue)
            {
                AddCriteria(fb => fb.UserId == userId.Value);
            }

            if (p.Status.HasValue)
            {
                AddCriteria(fb => fb.Status == p.Status.Value);
            }

            AddInclude(fb => fb.User);
            AddInclude(fb => fb.FlightSchedule);
            AddOrderByDescending(fb => fb.CreatedAt);
            ApplyPaging((p.PageNumber - 1) * p.PageSize, p.PageSize);
        }
    }
}
