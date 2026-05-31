
namespace Travel_Explorer.Application.Features.Activities
{
    /// <summary>
    /// Specification for querying activities with optional filters and includes.
    /// </summary>
    public class ActivitySpecification : BaseSpecification<Activity>
    {
        /// <summary>
        /// Get a single activity by ID (non-deleted) with Destination included.
        /// </summary>
        public ActivitySpecification(int id)
            : base(a => a.Id == id)
        {
            AddInclude(a => a.Destination);
        }

        /// <summary>
        /// Get all active activities, optionally filtered by destination (no paging).
        /// </summary>
        public ActivitySpecification(int? destinationId = null)
            : base(a => !destinationId.HasValue || a.DestinationId == destinationId.Value)
        {
            AddInclude(a => a.Destination);
            AddOrderBy(a => a.Id);

        }

        /// <summary>
        /// Get a paged list of active activities, optionally filtered by destination.
        /// </summary>
        public ActivitySpecification(int? destinationId, int pageNumber, int pageSize)
            : base(a => !destinationId.HasValue || a.DestinationId == destinationId.Value)
        {
            AddInclude(a => a.Destination);
            AddOrderBy(a => a.Id);
            ApplyPaging((pageNumber - 1) * pageSize, pageSize);
        }
    }
}
