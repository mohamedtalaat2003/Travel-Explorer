
namespace Travel_Explorer.Application.Features.Reviews.Queries.GetReviewById
{

    
    
    
    public record GetReviewByIdQuery(int Id) : IRequest<ReviewDto?>;

}
