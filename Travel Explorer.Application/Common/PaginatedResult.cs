namespace Travel_Explorer.Application.Common
{
    /// <summary>
    /// A generic wrapper for paginated API responses.
    /// Used on endpoints that return list data with pagination metadata.
    /// </summary>
    /// <typeparam name="T">The DTO type of each item in the page.</typeparam>
    public class PaginatedResult<T>
    {
        /// <summary>The items on the current page.</summary>
        public IReadOnlyList<T> Items { get; set; }

        /// <summary>The current page number (1-indexed).</summary>
        public int PageNumber { get; set; }

        /// <summary>The number of items requested per page.</summary>
        public int PageSize { get; set; }

        /// <summary>Total number of items across all pages.</summary>
        public int TotalCount { get; set; }

        /// <summary>Total number of pages.</summary>
        public int TotalPages => (int)Math.Ceiling((double)TotalCount / PageSize);

        /// <summary>True if there is a previous page.</summary>
        public bool HasPreviousPage => PageNumber > 1;

        /// <summary>True if there is a next page.</summary>
        public bool HasNextPage => PageNumber < TotalPages;

        public PaginatedResult(IReadOnlyList<T> items, int totalCount, int pageNumber, int pageSize)
        {
            Items = items;
            TotalCount = totalCount;
            PageNumber = pageNumber;
            PageSize = pageSize;
        }
    }
}
