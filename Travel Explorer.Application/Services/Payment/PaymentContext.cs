namespace Travel_Explorer.Application.Services.Payment
{
    public class PaymentContext
    {
        public decimal Amount { get; set; }
        public string Currency { get; set; } = "EGP";
        public string MerchantOrderId { get; set; } = string.Empty;
        public BillingData Billing { get; set; } = new();
    }

    public class BillingData
    {
        public string FirstName { get; set; } = "NA";
        public string LastName { get; set; } = "NA";
        public string Email { get; set; } = "NA";
        public string PhoneNumber { get; set; } = "NA";
    }
}
