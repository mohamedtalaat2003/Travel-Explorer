
namespace Travel_Explorer.Domain.Entities
{
    /// <summary>
    /// Represents a travel destination (e.g., a hotel, a resort, or a city landmark).
    /// This is the primary entity for bookings and reviews.
    /// </summary>
    public class Destination : BaseEntity
    {
        /// <summary>
        /// Name of the destination (e.g., "Sharm El Sheikh Resort").
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Detailed description of the destination.
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// The geographic location or address (e.g., "Egypt, South Sinai").
        /// </summary>
        public string Location { get; set; }

        /// <summary>
        /// The price per night for booking this destination.
        /// </summary>
        public decimal PricePerNight { get; set; }

        /// <summary>
        /// Average rating calculated from all user reviews (e.g., 4.5).
        /// </summary>
        public double AverageRating { get; set; }

        /// <summary>
        /// Number of reviews received for this destination.
        /// </summary>
        public int ReviewCount { get; set; }

        /// <summary>
        /// Collection of image URLs for the destination gallery.
        /// </summary>
        public List<string> ImageUrls { get; set; } = [];

        // ===== Foreign Keys =====

        /// <summary>
        /// Foreign key to the category this destination belongs to.
        /// </summary>
        public int CategoryId { get; set; }

        // ===== Navigation Properties =====

        /// <summary>
        /// The category this destination belongs to.
        /// </summary>
        public Category Category { get; set; }

        /// <summary>
        /// All activities available at this destination.
        /// </summary>
        public ICollection<Activity> Activities { get; set; } = new List<Activity>();

        /// <summary>
        /// All bookings made for this destination.
        /// </summary>
        public ICollection<DestinationBooking> Bookings { get; set; } = new List<DestinationBooking>();

        /// <summary>
        /// All reviews written for this destination.
        /// </summary>
        public ICollection<Review> Reviews { get; set; } = new List<Review>();
    }
}

