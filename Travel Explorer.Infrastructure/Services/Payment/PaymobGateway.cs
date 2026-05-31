using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Security.Cryptography;
using System.Text;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Travel_Explorer.Application.Services.Payment;

namespace Travel_Explorer.Infrastructure.Services.Payment
{
    public class PaymobGateway : IPaymentGateway
    {
        public string ProviderName => "Paymob";

        private readonly PaymobtSettings _settings;
        private readonly HttpClient _httpClient;
        private readonly ILogger<PaymobGateway> _logger;

        public PaymobGateway(
            IOptions<PaymobtSettings> settings,
            HttpClient httpClient,
            ILogger<PaymobGateway> logger)
        {
            _settings = settings.Value;
            _httpClient = httpClient;
            _logger = logger;

            _httpClient.BaseAddress = new Uri("https://accept.paymob.com");
            _httpClient.DefaultRequestHeaders.Authorization =
                new AuthenticationHeaderValue("Bearer", _settings.Secretkey);
        }

        public async Task<PaymentResult> CreateCheckoutAsync(
            PaymentContext context,
            CancellationToken cancellationToken = default)
        {
            var amountCents = (int)(context.Amount * 100);

            _logger.LogInformation("[Paymob] Creating payment intention for Order={MerchantOrderId}, Amount={Amount}", 
                context.MerchantOrderId, context.Amount);

            var response = await _httpClient.PostAsJsonAsync("/api/intention/", new
            {
                amount = amountCents,
                currency = context.Currency,
                payment_methods = new[] { _settings.PaymentMethodId },
                items = Array.Empty<object>(),
                billing_data = new
                {
                    first_name = context.Billing.FirstName,
                    last_name = context.Billing.LastName,
                    email = context.Billing.Email,
                    phone_number = context.Billing.PhoneNumber
                },
                special_reference = context.MerchantOrderId
            }, cancellationToken);

            if (!response.IsSuccessStatusCode)
            {
                var error = await response.Content.ReadAsStringAsync(cancellationToken);
                _logger.LogError("[Paymob] Intention API failed: {Status} - {Error}", response.StatusCode, error);
                return PaymentResult.Failure($"Intention API failed: {response.StatusCode}");
            }

            var resultDoc = await response.Content.ReadFromJsonAsync<JsonElement>(cancellationToken: cancellationToken);
            var clientSecret = resultDoc.GetProperty("client_secret").GetString();
            var intentionId = resultDoc.GetProperty("id").GetInt64().ToString();

            var checkoutUrl = $"https://accept.paymob.com/unifiedcheckout/?publicKey={_settings.PublicKey}&clientSecret={clientSecret}";

            return PaymentResult.Success(checkoutUrl, intentionId);
        }

        public Task<WebhookResult> VerifyWebhookAsync(
            IDictionary<string, string> headers,
            string body,
            CancellationToken cancellationToken = default)
        {
            headers.TryGetValue("hmac", out var hmac);

            if (string.IsNullOrEmpty(hmac) || string.IsNullOrEmpty(body))
                return Task.FromResult(WebhookResult.Invalid("Missing HMAC or body"));

            try
            {
                var doc = JsonDocument.Parse(body);
                var obj = doc.RootElement.GetProperty("obj");

                // Paymob HMAC is computed from this exact ordered set of fields.
                var orderObj = obj.GetProperty("order");
                var sourceData = obj.GetProperty("source_data");

                var fields = new[]
                {
                    obj.GetProperty("amount_cents").GetInt64().ToString(),
                    obj.GetProperty("created_at").GetString()!,
                    obj.GetProperty("currency").GetString()!,
                    obj.GetProperty("error_occured").GetBoolean().ToString().ToLower(),
                    obj.GetProperty("has_parent_transaction").GetBoolean().ToString().ToLower(),
                    obj.GetProperty("id").GetInt64().ToString(),
                    obj.GetProperty("integration_id").GetInt64().ToString(),
                    obj.GetProperty("is_3d_secure").GetBoolean().ToString().ToLower(),
                    obj.GetProperty("is_auth").GetBoolean().ToString().ToLower(),
                    obj.GetProperty("is_capture").GetBoolean().ToString().ToLower(),
                    obj.GetProperty("is_refunded").GetBoolean().ToString().ToLower(),
                    obj.GetProperty("is_standalone_payment").GetBoolean().ToString().ToLower(),
                    obj.GetProperty("is_voided").GetBoolean().ToString().ToLower(),
                    orderObj.GetProperty("id").GetInt64().ToString(),
                    obj.GetProperty("owner").GetInt64().ToString(),
                    sourceData.GetProperty("pan").GetString()!,
                    sourceData.GetProperty("sub_type").GetString()!,
                    sourceData.GetProperty("type").GetString()!,
                    obj.GetProperty("success").GetBoolean().ToString().ToLower()
                };

                var concatenated = string.Concat(fields);

                using var hmacAlg = new HMACSHA512(Encoding.UTF8.GetBytes(_settings.HmacSecret));
                var computed = Convert.ToHexString(hmacAlg.ComputeHash(Encoding.UTF8.GetBytes(concatenated))).ToLower();

                var isValid = CryptographicOperations.FixedTimeEquals(
                    Encoding.UTF8.GetBytes(computed),
                    Encoding.UTF8.GetBytes(hmac.ToLower()));

                if (!isValid)
                    return Task.FromResult(WebhookResult.Invalid("HMAC mismatch"));

                var success = obj.GetProperty("success").GetBoolean();
                var txId = obj.GetProperty("id").GetInt64().ToString();
                var merchantOrderId = obj.GetProperty("order").GetProperty("merchant_order_id").GetString() ?? "";

                return Task.FromResult(WebhookResult.Valid(success, merchantOrderId, txId));
            }
            catch (Exception ex)
            {
                _logger.LogWarning(ex, "[Paymob] HMAC verification exception");
                return Task.FromResult(WebhookResult.Invalid("Verification error"));
            }
        }
    }
}
