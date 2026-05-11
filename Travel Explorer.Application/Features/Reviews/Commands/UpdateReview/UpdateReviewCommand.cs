
using System.Text.Json.Serialization;

namespace Travel_Explorer.Application.Features.Reviews.Commands.UpdateReview
{
    /// <summary>
    /// Updates an existing review.
    /// </summary>
    public record UpdateReviewCommand(
        [Required]
        [Range(1, 5, ErrorMessage = "Rating must be between 1 and 5")]
        int Rating,

        [StringLength(1000)]
        string? Comment) : IRequest<ReviewDto?>
    {
        [JsonIgnore]
        public int Id { get; set; }
    }
}
