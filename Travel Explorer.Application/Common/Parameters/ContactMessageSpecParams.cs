namespace Travel_Explorer.Application.Common.Parameters
{
    public class ContactMessageSpecParams : PaginationParams
    {
        public bool? IsRead { get; set; }
        public string? Keyword { get; set; }
    }
}
