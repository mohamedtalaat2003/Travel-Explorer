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
            : base(b => b.Id == id && !b.IsDeleted)
        {
            AddInclude(b => b.User);
            AddInclude(b => b.Destination);
        }

        /// <summary>
        /// Get bookings filtered by optional UserId and/or Status.
        /// </summary>
        public DestinationBookingSpecification(int? userId, string? status)
            : base(b => !b.IsDeleted
                && (!userId.HasValue || b.UserId == userId.Value)
                && (string.IsNullOrEmpty(status) || b.Status.ToString() == status))
        {
            AddInclude(b => b.User);
            AddInclude(b => b.Destination);
            AddOrderByDescending(b => b.CreatedAt);
        
    }
}
}
