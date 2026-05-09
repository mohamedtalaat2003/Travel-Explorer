
namespace Travel_Explorer.Application.Features.Reviews.Queries.GetReviewById
{

    /// <summary>
    /// Returns a single review by its ID.
    /// </summary>
    public record GetReviewByIdQuery(int Id) : IRequest<ReviewDto?>;

}
