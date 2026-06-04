
namespace Travel_Explorer.Application.Features.Reviews.Commands.DeleteReview
{
    public class DeleteReviewCommandHandler(IUnitOfWork unitOfWork)
                : IRequestHandler<DeleteReviewCommand, bool>
    {
        private readonly IUnitOfWork _unitOfWork = unitOfWork;

        public async Task<bool> Handle(
            DeleteReviewCommand request, CancellationToken cancellationToken)
        {
            var review = await _unitOfWork.Repository<Review>().GetAsync(request.Id) ?? throw new NotFoundException(nameof(Review), request.Id);
            review.IsDeleted = true;
            review.UpdatedAt = DateTime.UtcNow;

            
            var destination = await _unitOfWork.Repository<Destination>().GetAsync(review.DestinationId);
            if (destination != null)
            {
                var reviewSpec = new BaseSpecification<Review>(
                    r => r.DestinationId == review.DestinationId && r.Id != review.Id);
                var remainingReviews = await _unitOfWork.Repository<Review>().ListSpecAsync(reviewSpec);

                destination.ReviewCount = remainingReviews.Count;
                destination.AverageRating = remainingReviews.Count > 0
                    ? remainingReviews.Average(r => r.Rating)
                    : 0;
                destination.UpdatedAt = DateTime.UtcNow;
            }

            await _unitOfWork.SaveChangesAsync(cancellationToken);
            return true;
        
    }
}
}
