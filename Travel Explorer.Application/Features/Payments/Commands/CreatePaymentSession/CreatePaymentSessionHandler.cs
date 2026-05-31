using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.Extensions.Logging;
using Travel_Explorer.Application.Services.Payment;
using Travel_Explorer.Domain.Entities;
using Travel_Explorer.Domain.Enums;
using Travel_Explorer.Domain.Interfaces;

namespace Travel_Explorer.Application.Features.Payments.Commands.CreatePaymentSession
{
    public class CreatePaymentSessionHandler : IRequestHandler<CreatePaymentSessionCommand, CreatePaymentSessionResult>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IPaymentGatewayFactory _gatewayFactory;
        private readonly ILogger<CreatePaymentSessionHandler> _logger;

        public CreatePaymentSessionHandler(
            IUnitOfWork unitOfWork,
            IPaymentGatewayFactory gatewayFactory,
            ILogger<CreatePaymentSessionHandler> logger)
        {
            _unitOfWork = unitOfWork;
            _gatewayFactory = gatewayFactory;
            _logger = logger;
        }

        public async Task<CreatePaymentSessionResult> Handle(
            CreatePaymentSessionCommand request,
            CancellationToken cancellationToken)
        {
            IPaymentGateway gateway;
            try
            {
                gateway = _gatewayFactory.GetGateway(request.Provider);
            }
            catch (System.NotSupportedException ex)
            {
                return CreatePaymentSessionResult.Fail(ex.Message);
            }

            var booking = await _unitOfWork.Repository<DestinationBooking>()
                .GetAsync(request.BookingId);

            if (booking is null)
                return CreatePaymentSessionResult.Fail("Booking not found.");

            if (booking.UserId != request.UserId)
                return CreatePaymentSessionResult.Fail("You do not own this booking.");

            if (booking.Status != BookingStatus.Pending)
                return CreatePaymentSessionResult.Fail($"Booking status is '{booking.Status}', expected 'Pending'.");

            var paymentTx = new PaymentTransaction
            {
                Amount = booking.TotalPrice,
                Status = PaymentStatus.Unpaid,
                PaymentMethod = gateway.ProviderName,
                UserId = request.UserId
            };

            await _unitOfWork.Repository<PaymentTransaction>().AddAsync(paymentTx);
            await _unitOfWork.SaveChangesAsync(cancellationToken);

            booking.PaymentId = paymentTx.Id;
            _unitOfWork.Repository<DestinationBooking>().Update(booking);
            await _unitOfWork.SaveChangesAsync(cancellationToken);

            _logger.LogInformation(
                "PaymentTx {TxId} created via {Provider} for Booking {BookingId}",
                paymentTx.Id, gateway.ProviderName, booking.Id);

            var user = await _unitOfWork.Repository<ApplicationUser>().GetAsync(request.UserId);
            var fullName = string.IsNullOrWhiteSpace(user?.FullName) ? "Traveler" : user!.FullName;
            var nameParts = fullName.Split(' ', 2, StringSplitOptions.RemoveEmptyEntries);

            var context = new PaymentContext
            {
                Amount = booking.TotalPrice,
                Currency = "EGP",
                MerchantOrderId = paymentTx.Id.ToString(),
                Billing = new BillingData
                {
                    FirstName = nameParts.Length > 0 ? nameParts[0] : "Traveler",
                    LastName = nameParts.Length > 1 ? nameParts[1] : "NA",
                    Email = string.IsNullOrWhiteSpace(user?.Email) ? "no-reply@travelexplorer.com" : user!.Email,
                    PhoneNumber = string.IsNullOrWhiteSpace(user?.PhoneNumber) ? "NA" : user!.PhoneNumber
                }
            };

            var result = await gateway.CreateCheckoutAsync(context, cancellationToken);

            if (!result.IsSuccess)
            {
                _logger.LogError("[{Provider}] Checkout failed for Tx {TxId}: {Error}",
                    gateway.ProviderName, paymentTx.Id, result.ErrorMessage);
                return CreatePaymentSessionResult.Fail(result.ErrorMessage!);
            }

            return CreatePaymentSessionResult.Ok(result.CheckoutUrl!);
        }
    }
}
