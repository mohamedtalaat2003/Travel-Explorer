using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Travel_Explorer.Domain.Common;

namespace Travel_Explorer.Domain.Entities
{/// <summary>
 /// Represents a contact message submitted by a visitor or registered user.
 /// Used for the "Contact Us" form on the website.
 /// Admins can mark messages as read and soft-delete (archive) them.
 /// </summary>
    public class ContactMessage : BaseEntity
    {
        /// <summary>
        /// The full name of the person who sent the message.
        /// </summary>
        public string FullName { get; set; }

        /// <summary>
        /// The email address of the sender (used for replying).
        /// </summary>
        public string Email { get; set; }

        /// <summary>
        /// The subject/topic of the message (e.g., "Booking Issue", "Partnership Inquiry").
        /// </summary>
        public string Subject { get; set; }

        /// <summary>
        /// The full text body of the contact message.
        /// </summary>
        public string Message { get; set; }

        /// <summary>
        /// Whether an admin has read this message.
        /// Default is false (unread). Used for inbox management.
        /// </summary>
        public bool IsRead { get; set; } = false;

        // ===== Foreign Keys =====

        /// <summary>
        /// Foreign key to the registered user who sent this message (optional).
        /// Null if the sender is a guest/visitor who is not logged in.
        /// </summary>
        public int? UserId { get; set; }

        // ===== Navigation Properties =====

        /// <summary>
        /// The registered user who sent this message (nullable).
        /// </summary>
        public ApplicationUser? User { get; set; }
    }
}
