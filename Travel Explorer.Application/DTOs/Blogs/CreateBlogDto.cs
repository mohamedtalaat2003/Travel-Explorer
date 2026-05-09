
namespace Travel_Explorer.Application.DTOs.Blogs
{
    public class CreateBlogDto
    {
        [Required(ErrorMessage = "Blog title is required")]
        [StringLength(250, MinimumLength = 5)]
        public string Title { get; set; }

        [Required(ErrorMessage = "Content is required")]
        public string Content { get; set; }

        [StringLength(500)]
        public string ImageUrl { get; set; }

        public bool IsPublished { get; set; }

        public int? CategoryId { get; set; }
    }
}
