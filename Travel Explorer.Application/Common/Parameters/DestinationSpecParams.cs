namespace Travel_Explorer.Application.Common.Parameters
{
    public class DestinationSpecParams : PaginationParams
    {
        public string? Keyword { get; set; }
        public string? Location { get; set; }
        public decimal? MinPrice { get; set; }
        public decimal? MaxPrice { get; set; }
        public int? CategoryId { get; set; }
    }
}
