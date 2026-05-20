namespace Travel_Explorer.Application.Common.Parameters
{
    public class BlogSpecParams : PaginationParams
    {
        public string? Keyword { get; set; }
        public int? AuthorId { get; set; }
        public int? CategoryId { get; set; }
    }
}
