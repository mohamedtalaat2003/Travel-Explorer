using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Travel_Explorer.Application.Services.Payment
{
    public class WebhookResult
    {
        public bool IsValid { get; set; }
        public bool IsPaymentSuccessful { get; set; }
        public string? MerchantOrderId { get; set; }
        public string? ProviderTransactionId { get; set; }
        public string? ErrorMessage { get; set; }

        public static WebhookResult Valid(bool success, string merchantOrderId, string providerTransactionId)
        => new()
        {
            IsValid = true,
            IsPaymentSuccessful = success,
            MerchantOrderId = merchantOrderId,
            ProviderTransactionId = providerTransactionId
        };

        public static WebhookResult Invalid(string errorMessage) => new()
        { IsValid = false , ErrorMessage = errorMessage };


    }
}
