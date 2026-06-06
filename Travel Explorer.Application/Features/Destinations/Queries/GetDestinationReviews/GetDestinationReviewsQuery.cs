namespace Travel_Explorer.Application.Features.Destinations.Queries.GetDestinationReviews
{

    
    
    
    public record GetDestinationReviewsQuery(int DestinationId) : IRequest<IReadOnlyList<ReviewDto>>;

}
