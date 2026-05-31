using Travel_Explorer.Application.Common;

namespace Travel_Explorer.Application.Features.Reviews.Commands.CreateReview
{
    public class CreateReviewCommandHandler(IUnitOfWork unitOfWork, IMapper mapper, ICurrentUserService currentUserService)
                : IRequestHandler<CreateReviewCommand, ReviewDto>
    {
        private readonly IUnitOfWork _unitOfWork = unitOfWork;
        private readonly IMapper _mapper = mapper;
        private readonly ICurrentUserService _currentUserService = currentUserService;

        public async Task<ReviewDto> Handle(
            CreateReviewCommand request, CancellationToken cancellationToken)
        {
            var userId = _currentUserService.UserId ?? throw new UnauthorizedAccessException();

            // The destination being reviewed must exist.
            var destination = await _unitOfWork.Repository<Destination>().GetAsync(request.DestinationId);
            if (destination is null || destination.IsDeleted)
                throw new NotFoundException(nameof(Destination), request.DestinationId);

            // A user may review a given destination only once.
            var duplicateSpec = new BaseSpecification<Review>(
                r => r.UserId == userId && r.DestinationId == request.DestinationId);
            var existingReview = await _unitOfWork.Repository<Review>().GenericEntitiesWithSpec(duplicateSpec);
            if (existingReview is not null)
                throw new ConflictException("You have already reviewed this destination.");

            var review = _mapper.Map<Review>(request);
            review.UserId = userId;
            review.CreatedAt = DateTime.UtcNow;

            await _unitOfWork.Repository<Review>().AddAsync(review);
            await _unitOfWork.SaveChangesAsync(cancellationToken);

            // Update destination average rating and review count
            await RecalculateDestinationRating(request.DestinationId);

            await _unitOfWork.SaveChangesAsync(cancellationToken);

            // Reload with includes
            var spec = new ReviewSpecification(review.Id);
            var loaded = await _unitOfWork.Repository<Review>().GenericEntitiesWithSpec(spec);

            return _mapper.Map<ReviewDto>(loaded);
        }

        private async Task RecalculateDestinationRating(int destinationId)
        {
            var destination = await _unitOfWork.Repository<Destination>().GetAsync(destinationId);
            if (destination == null) return;

            var reviewSpec = new ReviewSpecification(destinationId, true);
            var reviews = await _unitOfWork.Repository<Review>().ListSpecAsync(reviewSpec);

            destination.ReviewCount = reviews.Count;
            destination.AverageRating = reviews.Count > 0
                ? reviews.Average(r => r.Rating)
                : 0;
            destination.UpdatedAt = DateTime.UtcNow;
        
        }
    }
}
