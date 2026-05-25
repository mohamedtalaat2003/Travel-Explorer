using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Travel_Explorer.Application.Services.Payment
{
    public class PaymentResult
    {
        public bool IsSuccessful { get; init; }
        public string? CheckoutUrl { get; init; }
        public string? ErrorMessage { get; init; }
        public string? ProviderOrderId { get; init; }

        public static PaymentResult Success(string checkoutUrl, string providerOrderId)
        {
            return new PaymentResult
            {
                IsSuccessful = true,
                CheckoutUrl = checkoutUrl,
                ProviderOrderId = providerOrderId
            };
        }

        public static PaymentResult Failure(string errorMessage)
        {
            return new PaymentResult
            {
                IsSuccessful = false,
                ErrorMessage = errorMessage
            };
        }
    }
}
