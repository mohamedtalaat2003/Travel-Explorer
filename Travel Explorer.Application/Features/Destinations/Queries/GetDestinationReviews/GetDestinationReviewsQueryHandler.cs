using Travel_Explorer.Application.Features.Reviews;

namespace Travel_Explorer.Application.Features.Destinations.Queries.GetDestinationReviews
{
    public class GetDestinationReviewsQueryHandler(IUnitOfWork unitOfWork, IMapper mapper)
                : IRequestHandler<GetDestinationReviewsQuery, IReadOnlyList<ReviewDto>>
    {
        private readonly IUnitOfWork _unitOfWork = unitOfWork;
        private readonly IMapper _mapper = mapper;

        public async Task<IReadOnlyList<ReviewDto>> Handle(
            GetDestinationReviewsQuery request, CancellationToken cancellationToken)
        {
            var spec = new ReviewSpecification(request.DestinationId, true);
            var reviews = await _unitOfWork.Repository<Review>().ListSpecAsync(spec);

            return _mapper.Map<IReadOnlyList<ReviewDto>>(reviews);
        }
    
    }
}
