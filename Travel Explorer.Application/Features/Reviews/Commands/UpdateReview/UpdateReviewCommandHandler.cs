
namespace Travel_Explorer.Application.Features.Reviews.Commands.UpdateReview
{
    public class UpdateReviewCommandHandler
        : IRequestHandler<UpdateReviewCommand, ReviewDto?>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public UpdateReviewCommandHandler(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<ReviewDto?> Handle(
            UpdateReviewCommand request, CancellationToken cancellationToken)
        {
            var spec = new ReviewSpecification(request.Id);
            var review = await _unitOfWork.Repository<Review>().GenericEntitiesWithSpec(spec);

            if (review == null)
                throw new NotFoundException(nameof(Review), request.Id);

            // Only the review owner can update it
            if (review.UserId != request.UserId)
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
