using System.ComponentModel.DataAnnotations;
using Travel_Explorer.Domain.Enums;

namespace Travel_Explorer.Domain.Entities
{
    /// <summary>
    /// Represents a flight booking made by a user.
    /// This is a transactional entity - records must never be hard-deleted
    /// for audit and legal compliance purposes.
    /// </summary>
    public class FlightBooking : BaseEntity
    {
        /// <summary>
        /// The class of the flight ticket (Economy, Business, or FirstClass).
        /// Determines which price tier is applied from FlightSchedule.
        /// </summary>
        public FlightClass Class { get; set; }

        /// <summary>
        /// The number of passengers included in this booking.
        /// </summary>
        public int NumberOfPassengers { get; set; }

        /// <summary>
        /// The total price for all passengers in the selected class.
        /// </summary>
        public decimal TotalPrice { get; set; }

        /// <summary>
        /// The current status of this booking (Pending, Confirmed, Cancelled, Completed, Refunded).
        /// Default is Pending until payment is confirmed.
        /// </summary>
        public BookingStatus Status { get; set; } = BookingStatus.Pending;

        /// <summary>
        /// Optional seat preference or special requests (e.g., "window seat", "extra legroom").
        /// </summary>
        public string? SeatPreference { get; set; }

        public Gender Gender { get; set; }
        public string? Nationality { get; set; }

        // ===== Foreign Keys =====

        /// <summary>
        /// Foreign key to the user who made this flight booking.
        /// </summary>
        public int UserId { get; set; }

        /// <summary>
        /// Foreign key to the flight schedule being booked.
        /// </summary>
        public int FlightScheduleId { get; set; }

        /// <summary>
        /// Foreign key to the payment transaction associated with this booking (nullable).
        /// </summary>
        public int? PaymentId { get; set; }

        // ===== Navigation Properties =====

        /// <summary>
        /// The user who made this flight booking.
        /// </summary>
        public ApplicationUser User { get; set; }

        /// <summary>
        /// The flight schedule associated with this booking.
        /// </summary>
        public FlightSchedule FlightSchedule { get; set; }

        /// <summary>
        /// The payment transaction associated with this booking (nullable if unpaid).
        /// </summary>
        public PaymentTransaction? Payment { get; set; }
        [Timestamp]
        public byte[] Version { get; set; }


    }
}
