using Travel_Explorer.Domain.Enums;

using Microsoft.AspNetCore.Identity;
namespace Travel_Explorer.Domain.Entities
{
    /// <summary>
    /// Represents a registered user in the system.
    /// Inherits from IdentityUser (not BaseEntity) because C# does not allow multiple inheritance.
    /// Audit fields (CreatedAt, UpdatedAt, IsDeleted) are added manually.
    /// </summary>
    public class ApplicationUser : IdentityUser<int>
    {
        /// <summary>
        /// The full display name of the user (e.g., "Ahmed Mohamed").
        /// </summary>
        public string FullName { get; set; }

        /// <summary>
        /// The role assigned to this user (Admin, Traveler, or ContentWriter).
        /// Determines what features and pages the user can access.
        /// </summary>
        public UserRole Role { get; set; }

        /// <summary>
        /// The date and time when this user account was created.
        /// </summary>
        public DateTime CreatedAt { get; set; }

        /// <summary>
        /// The date and time when this user account was last modified.
        /// Null if the account has never been updated after creation.
        /// </summary>
        public DateTime? UpdatedAt { get; set; }

        /// <summary>
        /// Soft-delete flag. If true, the user is considered deactivated
        /// but still exists in the database. Default is false.
        /// </summary>
        public bool IsDeleted { get; set; } = false;

        // ===== Navigation Properties =====

        /// <summary>
        /// The detailed profile information for this user (one-to-one relationship).
        /// </summary>
        public UserProfile Profile { get; set; }

        /// <summary>
        /// All destination bookings made by this user.
        /// </summary>
        public ICollection<DestinationBooking> DestinationBookings { get; set; } = new List<DestinationBooking>();

        /// <summary>
        /// All flight bookings made by this user.
        /// </summary>
        public ICollection<FlightBooking> FlightBookings { get; set; } = new List<FlightBooking>();

        /// <summary>
        /// All reviews written by this user on destinations.
        /// </summary>
        public ICollection<Review> Reviews { get; set; } = new List<Review>();

        /// <summary>
        /// All blog posts authored by this user (ContentWriter role).
        /// </summary>
        public ICollection<Blog> Blogs { get; set; } = new List<Blog>();
    }
}
