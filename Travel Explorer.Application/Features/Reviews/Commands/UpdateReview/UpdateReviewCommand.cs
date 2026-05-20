
using System.Text.Json.Serialization;

namespace Travel_Explorer.Application.Features.Reviews.Commands.UpdateReview
{
    /// <summary>
    /// Updates an existing review.
    /// </summary>
    public record UpdateReviewCommand(
        int Rating,
        string? Comment,
        List<string>? ImageUrls = null) : IRequest<ReviewDto?>
    {
        [JsonIgnore]
        public int Id { get; set; }
    }
}
