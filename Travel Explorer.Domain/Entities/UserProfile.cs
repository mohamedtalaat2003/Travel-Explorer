using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Travel_Explorer.Domain.Common;

namespace Travel_Explorer.Domain.Entities
{
    /// <summary>
    /// Represents a user's detailed profile information.
    /// Separated from ApplicationUser to keep identity logic clean.
    /// Has a one-to-one relationship with ApplicationUser via UserId.
    /// </summary>
    public class UserProfile : BaseEntity
    {
        /// <summary>
        /// A short biography or "about me" text written by the user (optional).
        /// </summary>
        public string? Bio { get; set; }

        /// <summary>
        /// URL or path to the user's profile/avatar image (optional).
        /// </summary>
        public string? AvatarUrl { get; set; }

        /// <summary>
        /// The user's phone number for contact purposes (optional).
        /// </summary>
        public string? PhoneNumber { get; set; }

        /// <summary>
        /// The user's country of residence (optional).
        /// </summary>
        public string? Country { get; set; }

        /// <summary>
        /// The user's date of birth (optional).
        /// Used for age-restricted bookings or personalized offers.
        /// </summary>
        public DateTime? DateOfBirth { get; set; }

        /// <summary>
        /// The user's passport number for international flight bookings (optional).
        /// Stored encrypted in the database for security compliance.
        /// </summary>
        public string? PassportNumber { get; set; }

        // ===== Foreign Keys =====

        /// <summary>
        /// Foreign key to the ApplicationUser this profile belongs to.
        /// One-to-one relationship — each user has exactly one profile.
        /// </summary>
        public int UserId { get; set; }

        // ===== Navigation Properties =====

        /// <summary>
        /// The ApplicationUser this profile belongs to.
        /// </summary>
        public ApplicationUser User { get; set; }
    }
}
