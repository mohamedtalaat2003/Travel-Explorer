namespace Travel_Explorer.Application.Services.Payment
{
    public class PaymentResult
    {
        public bool IsSuccess { get; init; }
        public string? CheckoutUrl { get; init; }
        public string? ErrorMessage { get; init; }
        public string? ProviderOrderId { get; init; }

        public static PaymentResult Success(string url, string? providerOrderId = null)
            => new() { IsSuccess = true, CheckoutUrl = url, ProviderOrderId = providerOrderId };

        public static PaymentResult Failure(string error)
            => new() { IsSuccess = false, ErrorMessage = error };
    }
}
