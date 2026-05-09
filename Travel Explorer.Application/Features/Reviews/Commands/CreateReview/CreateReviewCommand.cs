using System.Text.Json.Serialization;

namespace Travel_Explorer.Application.Features.Reviews.Commands.CreateReview
{
    /// <summary>
    /// Submits a new review. 
    /// Note: UserId is populated from the authenticated context in the controller.
    /// </summary>
    public record CreateReviewCommand(
        [Required]
        [Range(1, 5)]
        int Rating,

        [Required]
        [StringLength(1000)]
        string Comment,

        [Required]
        int DestinationId) : IRequest<ReviewDto>
    {
        /// <summary>
        /// The ID of the user submitting the review. Set by the controller from JWT claims.
        /// </summary>
        [JsonIgnore] // Prevent client from sending this
        public int UserId { get; set; }
    }
}
