namespace Travel_Explorer.Application.DTOs.Reviews
{
    public class ReviewDto
    {
        public int Id { get; set; }
        public int Rating { get; set; }
        public string? Comment { get; set; }
        public int UserId { get; set; }
        public string UserFullName { get; set; }
        public int DestinationId { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
