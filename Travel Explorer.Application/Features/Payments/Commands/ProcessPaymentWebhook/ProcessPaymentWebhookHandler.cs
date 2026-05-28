using System;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.Extensions.Logging;
using Travel_Explorer.Domain.Entities;
using Travel_Explorer.Domain.Enums;
using Travel_Explorer.Domain.Interfaces;

namespace Travel_Explorer.Application.Features.Payments.Commands.ProcessPaymentWebhook
{
    public class ProcessPaymentWebhookHandler : IRequestHandler<ProcessPaymentWebhookCommand, bool>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogger<ProcessPaymentWebhookHandler> _logger;

        public ProcessPaymentWebhookHandler(
            IUnitOfWork unitOfWork,
            ILogger<ProcessPaymentWebhookHandler> logger)
        {
            _unitOfWork = unitOfWork;
            _logger = logger;
        }

        public async Task<bool> Handle(
            ProcessPaymentWebhookCommand request,
            CancellationToken cancellationToken)
        {
            var webhook = request.WebhookResult;

            if (!int.TryParse(webhook.MerchantOrderId, out int paymentTxId))
            {
                _logger.LogWarning("[{Provider}] Invalid MerchantOrderId: {Id}",
                    request.ProviderName, webhook.MerchantOrderId);
                return false;
            }

            var paymentTx = await _unitOfWork.Repository<PaymentTransaction>()
                .GetAsync(paymentTxId);

            if (paymentTx is null)
            {
                _logger.LogWarning("PaymentTransaction {TxId} not found", paymentTxId);
                return false;
            }

            if (webhook.IsPaymentSuccessful)
            {
                paymentTx.Status = PaymentStatus.Paid;
                paymentTx.PaidAt = DateTime.UtcNow;
                paymentTx.TransactionReference = webhook.ProviderTransactionId;

                _logger.LogInformation(
                    "[{Provider}] Payment SUCCESS — TxId={TxId}, ProviderTxId={PTxId}",
                    request.ProviderName, paymentTxId, webhook.ProviderTransactionId);

                if (paymentTx.DestinationBookId.HasValue)
                {
                    var booking = await _unitOfWork.Repository<DestinationBooking>()
                        .GetAsync(paymentTx.DestinationBookId.Value);

                    if (booking is not null)
                    {
                        booking.Status = BookingStatus.Confirmed;
                        _unitOfWork.Repository<DestinationBooking>().Update(booking);
                        _logger.LogInformation("Booking {BookingId} confirmed", booking.Id);
                    }
                }
            }
            else
            {
                paymentTx.Status = PaymentStatus.Failed;
                _logger.LogWarning("[{Provider}] Payment FAILED — TxId={TxId}", request.ProviderName, paymentTxId);
            }

            _unitOfWork.Repository<PaymentTransaction>().Update(paymentTx);
            await _unitOfWork.SaveChangesAsync(cancellationToken);

            return true;
        }
    }
}
