using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Travel_Explorer.Application.Services.Payment
{
    public interface IPaymentGateway
    {
        string ProviderName { get; }

        Task<PaymentResult> CreateCheckoutAsync(
            PaymentContext context,
            CancellationToken cancellationToken = default);

        Task<WebhookResult> VerifyWebhookAsync(
            IDictionary<string, string> headers,
            string body,
            CancellationToken cancellationToken = default);
    }
}
