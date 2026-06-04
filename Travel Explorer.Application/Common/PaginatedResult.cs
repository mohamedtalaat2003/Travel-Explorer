namespace Travel_Explorer.Application.Common
{
    
    
    
    
    
    public class PaginatedResult<T>(IReadOnlyList<T> items, int totalCount, int pageNumber, int pageSize)
    {
        
        public IReadOnlyList<T> Items { get; set; } = items;

        
        public int PageNumber { get; set; } = pageNumber;

        
        public int PageSize { get; set; } = pageSize;

        
        public int TotalCount { get; set; } = totalCount;

        
        public int TotalPages => (int)Math.Ceiling((double)TotalCount / PageSize);

        
        public bool HasPreviousPage => PageNumber > 1;

        
        public bool HasNextPage => PageNumber < TotalPages;
    }
}
