
namespace Travel_Explorer.Application.Features.Destinations
{
    /// <summary>
    /// Specification for querying destinations with optional filters, includes, sorting, and paging.
    /// </summary>
    public class DestinationSpecification : BaseSpecification<Destination>
    {
        /// <summary>
        /// Get a single destination by ID (non-deleted) with Category included.
        /// </summary>
        public DestinationSpecification(int id)
            : base(d => d.Id == id)
        {
            AddInclude(d => d.Category);
        }

        /// <summary>
        /// Paginated list of all active destinations.
        /// </summary>
        public DestinationSpecification(int? pageNumber = null, int? pageSize = null)
            : base()
        {
            AddInclude(d => d.Category);
            AddOrderBy(d => d.Id);
            if (pageNumber.HasValue && pageSize.HasValue)
                ApplyPaging((pageNumber.Value - 1) * pageSize.Value, pageSize.Value);
        }

        public DestinationSpecification(
            string? keyword,
            string? location,
            decimal? minPrice,
            decimal? maxPrice,
            int? categoryId)
        {
            if (!string.IsNullOrEmpty(keyword))
                AddCriteria(d => d.Name.Contains(keyword) || d.Description.Contains(keyword));

            if (!string.IsNullOrEmpty(location))
                AddCriteria(d => d.Location.Contains(location));

            if (minPrice.HasValue)
                AddCriteria(d => d.PricePerNight >= minPrice.Value);

            if (maxPrice.HasValue)
                AddCriteria(d => d.PricePerNight <= maxPrice.Value);

            if (categoryId.HasValue)
                AddCriteria(d => d.CategoryId == categoryId.Value);

            AddInclude(d => d.Category);
            AddOrderBy(d => d.Id);
        }

        /// <summary>
        /// Top-rated destinations ordered by AverageRating descending.
        /// </summary>
        public DestinationSpecification(int count, bool topRated)
       : base()
        {
            AddInclude(d => d.Category);

            if (topRated)
            {
                AddOrderByDescending(d => d.AverageRating);
            }

            ApplyPaging(0, count);
        }
    }
    }
