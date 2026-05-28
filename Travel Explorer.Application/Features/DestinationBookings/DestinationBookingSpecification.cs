
using Travel_Explorer.Domain.Enums;

namespace Travel_Explorer.Application.Features.DestinationBookings
{
    /// <summary>
    /// Specification for querying destination bookings with optional filters and includes.
    /// </summary>
    public class DestinationBookingSpecification : BaseSpecification<DestinationBooking>
    {
        /// <summary>
        /// Get a single booking by ID (non-deleted) with User and Destination included.
        /// </summary>
        public DestinationBookingSpecification(int id)
            : base(b => b.Id == id )
        {
            AddInclude(b => b.User);
            AddInclude(b => b.Destination);
        }

        /// <summary>
        /// Get bookings filtered by optional UserId and/or Status.
        /// </summary>
        public DestinationBookingSpecification(int? userId, string? status)
             : base(b => (!userId.HasValue || b.UserId == userId.Value))
        {
            if (!string.IsNullOrEmpty(status))
            {
                if (Enum.TryParse<BookingStatus>(status, true, out var parsedStatus))
                {
                    AddCriteria(b => b.Status == parsedStatus);
                }
                else
                {
                    // Force zero results on invalid status value
                    AddCriteria(b => false);
                }
            }

            AddInclude(b => b.User);
            AddInclude(b => b.Destination);
            AddOrderByDescending(b => b.CreatedAt);
        }
    }
}
