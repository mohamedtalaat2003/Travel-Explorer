using System.ComponentModel.DataAnnotations;

namespace Travel_Explorer.Application.DTOs.Reviews
{
    public class UpdateReviewDto
    {
        [Required]
        public int Id { get; set; }

        [Required]
        [Range(1, 5, ErrorMessage = "Rating must be between 1 and 5")]
        public int Rating { get; set; }

        [StringLength(1000)]
        public string? Comment { get; set; }
    }
}
