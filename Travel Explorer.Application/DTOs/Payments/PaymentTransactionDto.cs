using Travel_Explorer.Domain.Enums;

namespace Travel_Explorer.Application.DTOs.Payments
{
    public class PaymentTransactionDto
    {
        public int Id { get; set; }
        public decimal Amount { get; set; }
        public PaymentStatus Status { get; set; }
        public string PaymentMethod { get; set; }
        public string? TransactionReference { get; set; }
        public DateTime? PaidAt { get; set; }
        public int? DestinationBookId { get; set; }
        public int? FlightBookId { get; set; }
        public int UserId { get; set; }
    }
}
