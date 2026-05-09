using System.Text;
using Travel_Explorer.Domain.Enums;

namespace Travel_Explorer.Domain.Entities
{
    /// <summary>
    /// Represents a payment transaction linked to a booking.
    /// This is a financial entity — records must NEVER be hard-deleted.
    /// Full audit trail is legally required for all financial operations.
    /// </summary>
    public class PaymentTransaction : BaseEntity
    {
        /// <summary>
        /// The total amount paid or to be paid in this transaction.
        /// </summary>
        public decimal Amount { get; set; }

        /// <summary>
        /// The current payment status (Unpaid, Partial, Paid, Failed, Refunded).
        /// Default is Unpaid until a payment attempt is made.
        /// </summary>
        public PaymentStatus Status { get; set; } = PaymentStatus.Unpaid;

        /// <summary>
        /// The method used for payment (e.g., "Visa", "MasterCard", "PayPal", "Cash").
        /// </summary>
        public string PaymentMethod { get; set; }

        /// <summary>
        /// The reference/confirmation number from the external payment gateway (optional).
        /// Used for tracking and dispute resolution with the payment provider.
        /// </summary>
        public string? TransactionReference { get; set; }

        /// <summary>
        /// The exact date and time when the payment was successfully processed.
        /// Null if the payment has not been completed yet.
        /// </summary>
        public DateTime? PaidAt { get; set; }

        // ===== Foreign Keys =====

        /// <summary>
        /// Foreign key to the destination booking this payment is for (nullable).
        /// Null if this payment is for a flight booking instead.
        /// </summary>
        public int? DestinationBookId { get; set; }

        /// <summary>
        /// Foreign key to the flight booking this payment is for (nullable).
        /// Null if this payment is for a destination booking instead.
        /// </summary>
        public int? FlightBookId { get; set; }

        /// <summary>
        /// Foreign key to the user who made this payment.
        /// </summary>
        public int UserId { get; set; }

        // ===== Navigation Properties =====

        /// <summary>
        /// The destination booking associated with this payment (nullable).
        /// </summary>
        public DestinationBooking? DestinationBook { get; set; }

        /// <summary>
        /// The flight booking associated with this payment (nullable).
        /// </summary>
        public FlightBooking? FlightBook { get; set; }

        /// <summary>
        /// The user who made this payment.
        /// </summary>
        public ApplicationUser User { get; set; }
    }
}
