
namespace Travel_Explorer.Application.Features.Reviews.Commands.DeleteReview
{

    /// <summary>
    /// Soft-deletes a review. Also recalculates destination AverageRating and ReviewCount.
    /// </summary>
    public record DeleteReviewCommand(int Id) : IRequest<bool>;

}
