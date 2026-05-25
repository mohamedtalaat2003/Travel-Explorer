namespace Travel_Explorer.Domain.Entities
{
    /// <summary>
    /// Represents a user review/rating on a destination.
    /// Users can rate destinations and leave optional comments.
    /// Admins can soft-delete inappropriate reviews without losing the data.
    /// </summary>
    public class Review : BaseEntity
    {
        /// <summary>
        /// The star rating given by the user (1 to 5).
        /// 1 = Very Poor, 2 = Poor, 3 = Average, 4 = Good, 5 = Excellent.
        /// </summary>
        public int Rating { get; set; }

        /// <summary>
        /// An optional text comment/feedback left by the user (nullable).
        /// </summary>
        public string? Comment { get; set; }

        /// <summary>
        /// Collection of image URLs uploaded for the review.
        /// </summary>
        public List<string> ImageUrls { get; set; } = new List<string>();

        // ===== Foreign Keys =====

        /// <summary>
        /// Foreign key to the user who wrote this review.
        /// </summary>
        public int UserId { get; set; }

        /// <summary>
        /// Foreign key to the destination being reviewed.
        /// </summary>
        public int DestinationId { get; set; }

        // ===== Navigation Properties =====

        /// <summary>
        /// The user who wrote this review.
        /// </summary>
        public ApplicationUser User { get; set; }

        /// <summary>
        /// The destination that this review is about.
        /// </summary>
        public Destination Destination { get; set; }
    }
}
