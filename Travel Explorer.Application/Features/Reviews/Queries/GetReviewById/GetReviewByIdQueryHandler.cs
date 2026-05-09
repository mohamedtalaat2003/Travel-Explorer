
namespace Travel_Explorer.Application.Features.Reviews.Queries.GetReviewById
{
    public class GetReviewByIdQueryHandler
        : IRequestHandler<GetReviewByIdQuery, ReviewDto?>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public GetReviewByIdQueryHandler(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<ReviewDto?> Handle(
            GetReviewByIdQuery request, CancellationToken cancellationToken)
        {
            var spec = new ReviewSpecification(request.Id);
            var review = await _unitOfWork.Repository<Review>().GenericEntitiesWithSpec(spec);

            if (review == null)
                return null;

            return _mapper.Map<ReviewDto>(review);
        
    }
}
}
