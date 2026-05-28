using MediatR;

namespace Travel_Explorer.Application.Features.Payments.Commands.CreatePaymentSession
{
    public class CreatePaymentSessionCommand : IRequest<CreatePaymentSessionResult>
    {
        public int BookingId { get; set; }
        public int UserId { get; set; }
        public string Provider { get; set; } = "Paymob";
    }

    public class CreatePaymentSessionResult
    {
        public bool IsSuccess { get; init; }
        public string? CheckoutUrl { get; init; }
        public string? Error { get; init; }

        public static CreatePaymentSessionResult Ok(string url) => new() { IsSuccess = true, CheckoutUrl = url };
        public static CreatePaymentSessionResult Fail(string error) => new() { IsSuccess = false, Error = error };
    }
}
