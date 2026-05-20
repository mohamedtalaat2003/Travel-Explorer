using Travel_Explorer.Domain.Enums;

namespace Travel_Explorer.Domain.Entities
{/// <summary>
 /// Represents a destination booking made by a user.
 /// This is a transactional entity — records must never be hard-deleted
 /// for audit and legal compliance purposes.
 /// </summary>
    public class DestinationBooking : BaseEntity
    {
        /// <summary>
        /// The date when the guest checks in to the destination.
        /// </summary>
        public DateTime CheckInDate { get; set; }

        /// <summary>
        /// The date when the guest checks out of the destination.
        /// </summary>
        public DateTime CheckOutDate { get; set; }

        /// <summary>
        /// The number of guests included in this booking.
        /// </summary>
        public int NumberOfGuests { get; set; }

        /// <summary>
        /// The total price for the entire booking (calculated from PricePerNight × nights).
        /// </summary>
        public decimal TotalPrice { get; set; }

        /// <summary>
        /// The current status of this booking (Pending, Confirmed, Cancelled, Completed, Refunded).
        /// Default is Pending until payment is confirmed.
        /// </summary>
        public BookingStatus Status { get; set; } = BookingStatus.Pending;

        /// <summary>
        /// Optional notes or special requests from the user (e.g., "late check-in").
        /// </summary>
        public string? Notes { get; set; }

        // ===== Foreign Keys =====

        /// <summary>
        /// Foreign key to the user who made this booking.
        /// </summary>
        public int UserId { get; set; }

        /// <summary>
        /// Foreign key to the destination being booked.
        /// </summary>
        public int DestinationId { get; set; }

        // ===== Navigation Properties =====

        /// <summary>
        /// The user who made this booking.
        /// </summary>
        public ApplicationUser User { get; set; }

        /// <summary>
        /// The destination that was booked.
        /// </summary>
        public Destination Destination { get; set; }

        /// <summary>
        /// The payment transaction associated with this booking (nullable if unpaid).
        /// </summary>
        public PaymentTransaction? Payment { get; set; }
    }
}
