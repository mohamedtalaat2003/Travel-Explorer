namespace Travel_Explorer.Application.Features.Reviews.Commands.CreateReview
{
    
    
    
    
    public record CreateReviewCommand(
        int Rating,
        string Comment,
        int DestinationId,
        List<string>? ImageUrls = null) : IRequest<ReviewDto>;
}
