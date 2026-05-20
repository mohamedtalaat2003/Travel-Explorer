
namespace Travel_Explorer.Application.Features.Reviews.Queries.GetReviewById
{
    public class GetReviewByIdQueryHandler(IUnitOfWork unitOfWork, IMapper mapper)
                : IRequestHandler<GetReviewByIdQuery, ReviewDto?>
    {
        private readonly IUnitOfWork _unitOfWork = unitOfWork;
        private readonly IMapper _mapper = mapper;

        public async Task<ReviewDto?> Handle(
            GetReviewByIdQuery request, CancellationToken cancellationToken)
        {
            var spec = new ReviewSpecification(request.Id);
            var review = await _unitOfWork.Repository<Review>().GenericEntitiesWithSpec(spec) ?? throw new NotFoundException(nameof(Review), request.Id);
            return _mapper.Map<ReviewDto>(review);
        
    }
}
}
