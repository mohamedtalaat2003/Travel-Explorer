namespace Travel_Explorer.Application.Features.Reviews.Commands.CreateReview
{
    /// <summary>
    /// Submits a new review. 
    /// Note: UserId is populated from ICurrentUserService in the handler.
    /// </summary>
    public record CreateReviewCommand(
        int Rating,
        string Comment,
        int DestinationId,
        List<string>? ImageUrls = null) : IRequest<ReviewDto>;
}
