
namespace Travel_Explorer.Application.Features.Reviews
{
    /// <summary>
    /// Specification for querying reviews with optional filters and includes.
    /// </summary>
    public class ReviewSpecification : BaseSpecification<Review>
    {
        /// <summary>
        /// Get a single review by ID (non-deleted) with User included.
        /// </summary>
        public ReviewSpecification(int id)
            : base(r => r.Id == id && !r.IsDeleted)
        {
            AddInclude(r => r.User);
        }

        /// <summary>
        /// Get reviews filtered by destination, with User included.
        /// Pass filterByDestination = true to distinguish from the ID constructor.
        /// </summary>
        public ReviewSpecification(int destinationId, bool filterByDestination)
            : base(r => !r.IsDeleted && r.DestinationId == destinationId)
        {
            AddInclude(r => r.User);
            AddOrderByDescending(r => r.CreatedAt);
        
    }
}
}
