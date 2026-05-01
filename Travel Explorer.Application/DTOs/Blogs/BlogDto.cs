namespace Travel_Explorer.Application.DTOs.Blogs
{
    public class BlogDto
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Content { get; set; }
        public string ImageUrl { get; set; }
        public bool IsPublished { get; set; }
        public int AuthorId { get; set; }
        public string AuthorName { get; set; }
        public int? CategoryId { get; set; }
        public string? CategoryName { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
