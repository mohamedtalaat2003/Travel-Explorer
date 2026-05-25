using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Travel_Explorer.Application.DTOs;

namespace Travel_Explorer.Application.Services.Payment
{
    public interface IPaymentGateway
    {
        /// <summary>
        /// Unique provider name used by the factory to resolve this gateway.
        /// Examples: "Paymob", "Stripe", "Fawry"
        /// </summary>
        string ProviderName { get; }

        Task<PaymentResult> CreateCheckoutAsync(PaymentContext context,CancellationToken cancellation = default);
        bool VerifyHmacSignature(IDictionary<string,string> headers,string body, CancellationToken cancellationToken = default);
    }
}
