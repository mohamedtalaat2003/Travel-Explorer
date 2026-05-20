using Travel_Explorer.Application.Common;

namespace Travel_Explorer.Application.Features.Reviews.Commands.UpdateReview
{
    public class UpdateReviewCommandHandler(IUnitOfWork unitOfWork, IMapper mapper, ICurrentUserService currentUserService)
                : IRequestHandler<UpdateReviewCommand, ReviewDto?>
    {
        private readonly IUnitOfWork _unitOfWork = unitOfWork;
        private readonly IMapper _mapper = mapper;
        private readonly ICurrentUserService _currentUserService = currentUserService;

        public async Task<ReviewDto?> Handle(
            UpdateReviewCommand request, CancellationToken cancellationToken)
        {
            var spec = new ReviewSpecification(request.Id);
            var review = await _unitOfWork.Repository<Review>().GenericEntitiesWithSpec(spec) ?? throw new NotFoundException(nameof(Review), request.Id);

            // Only the review owner or an admin can update it
            if (!_currentUserService.IsAdmin && review.UserId != _currentUserService.UserId)
                throw new ForbiddenAccessException("You are not authorized to update this review.");

            _mapper.Map(request, review);
            review.UpdatedAt = DateTime.UtcNow;

            await _unitOfWork.SaveChangesAsync(cancellationToken);

            // Update destination rating
            await RecalculateDestinationRating(review.DestinationId, cancellationToken);

            await _unitOfWork.SaveChangesAsync(cancellationToken);

            // Reload with includes
            var reloadSpec = new ReviewSpecification(review.Id);
            var loaded = await _unitOfWork.Repository<Review>().GenericEntitiesWithSpec(reloadSpec);

            return _mapper.Map<ReviewDto>(loaded);
        }

        private async Task RecalculateDestinationRating(int destinationId, CancellationToken cancellationToken)
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
