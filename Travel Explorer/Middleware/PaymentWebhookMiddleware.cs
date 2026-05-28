using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Travel_Explorer.Application.Services.Payment;

namespace Travel_Explorer.Middleware
{
    public class PaymentWebhookMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<PaymentWebhookMiddleware> _logger;

        public PaymentWebhookMiddleware(RequestDelegate next, ILogger<PaymentWebhookMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }

        public async Task InvokeAsync(HttpContext context, IPaymentGatewayFactory factory)
        {
            var path = context.Request.Path.Value ?? "";
            if (!path.StartsWith("/api/payments/webhook/", StringComparison.OrdinalIgnoreCase))
            {
                await _next(context);
                return;
            }

            var providerName = path.Split('/').LastOrDefault() ?? "";
            
            IPaymentGateway gateway;
            try { gateway = factory.GetGateway(providerName); }
            catch { context.Response.StatusCode = 400; return; }

            context.Request.EnableBuffering();
            using var reader = new StreamReader(context.Request.Body, leaveOpen: true);
            var body = await reader.ReadToEndAsync();
            context.Request.Body.Position = 0;

            var headerDict = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);
            foreach (var h in context.Request.Headers) headerDict[h.Key] = h.Value.ToString();
            foreach (var q in context.Request.Query) headerDict[q.Key] = q.Value.ToString();

            var result = await gateway.VerifyWebhookAsync(headerDict, body);

            if (!result.IsValid)
            {
                context.Response.StatusCode = StatusCodes.Status401Unauthorized;
                return;
            }

            context.Items["WebhookResult"] = result;
            context.Items["WebhookProvider"] = providerName;

            await _next(context);
        }
    }

    public static class PaymentWebhookMiddlewareExtensions
    {
        public static IApplicationBuilder UsePaymentWebhookVerification(this IApplicationBuilder app)
            => app.UseMiddleware<PaymentWebhookMiddleware>();
    }
}
