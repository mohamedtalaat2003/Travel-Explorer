using MediatR;
using Travel_Explorer.Application.Services.Payment;

namespace Travel_Explorer.Application.Features.Payments.Commands.ProcessPaymentWebhook
{
    public class ProcessPaymentWebhookCommand : IRequest<bool>
    {
        public WebhookResult WebhookResult { get; set; } = null!;
        public string ProviderName { get; set; } = string.Empty;
    }
}
