namespace Travel_Explorer.Application.Services.Payment
{
    public class WebhookResult
    {
        public bool IsValid { get; init; }
        public bool IsPaymentSuccessful { get; init; }
        public string? MerchantOrderId { get; init; }
        public string? ProviderTransactionId { get; init; }
        public string? ErrorMessage { get; init; }

        public static WebhookResult Valid(bool success, string merchantOrderId, string providerTxId)
            => new()
            {
                IsValid = true,
                IsPaymentSuccessful = success,
                MerchantOrderId = merchantOrderId,
                ProviderTransactionId = providerTxId
            };

        public static WebhookResult Invalid(string error)
            => new() { IsValid = false, ErrorMessage = error };
    }
}
