
namespace Travel_Explorer.Application.DTOs.Payments
{
    public class CreatePaymentTransactionDto
    {
        [Required]
        [Range(0.01, 1000000)]
        public decimal Amount { get; set; }

        [Required]
        [StringLength(50)]
        public string PaymentMethod { get; set; }

        [StringLength(200)]
        public string? TransactionReference { get; set; }

        public int? DestinationBookId { get; set; }
        public int? FlightBookId { get; set; }
    }
}
