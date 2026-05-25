using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text;
using System.Threading.Tasks;
using Travel_Explorer.Application.Services.Payment;

namespace Travel_Explorer.Infrastructure.Repositories
{
    public class PaymobGateway : IPaymentGateway
    {
        public string ProviderName => "Paymob";
        private readonly PaymobtSettings _settings;
        private readonly HttpClient _httpClient;
        private readonly ILogger<PaymobGateway> _logger;

        public PaymobGateway(IOptions<PaymobtSettings> options, HttpClient httpClient, ILogger<PaymobGateway> logger)
        {
            this._settings = options.Value;
            this._httpClient = httpClient;
            this._logger = logger;

            _httpClient.BaseAddress = new Uri("https//accept.paymob.com");
            _httpClient.DefaultRequestHeaders.Authorization =
                new AuthenticationHeaderValue("Bearer", _settings.Secretkey);
        }


        public async Task<PaymentResult> CreateCheckoutAsync(PaymentContext context, CancellationToken cancellation = default)
        {
            var amountCents = (int)(context.Amount * 100);

            // register order
            _logger.LogInformation($"[Paymob] Registering order MerchantOrderId={context.MerchantOrderId}, Amount={context.Amount}{context.Currency}");

            var orderResponse = await _httpClient.PostAsJsonAsync("/api")
        }

        public bool VerifyHmacSignature(IDictionary<string, string> headers, string body, CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }
    }
}
