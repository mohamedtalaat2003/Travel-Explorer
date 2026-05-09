using Travel_Explorer.Application.Features.Reviews;

namespace Travel_Explorer.Application.Features.Destinations.Queries.GetDestinationReviews
{

    /// <summary>
    /// Returns all reviews for a specific destination.
    /// </summary>
    public record GetDestinationReviewsQuery(int DestinationId) : IRequest<IReadOnlyList<ReviewDto>>;

}
