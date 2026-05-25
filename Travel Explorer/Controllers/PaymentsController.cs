using System.Security.Claims;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Travel_Explorer.Application.Features.Payments.Commands.CreatePaymentSession;
using Travel_Explorer.Application.Features.Payments.Commands.ProcessPaymentWebhook;
using Travel_Explorer.Application.Services.Payment;

namespace Travel_Explorer.Controllers
{
    [ApiController]
    [Route("api/payments")]
    [Produces("application/json")]
    public class PaymentsController : ControllerBase
    {
        private readonly IMediator _mediator;

        public PaymentsController(IMediator mediator) => _mediator = mediator;

        [HttpPost("checkout/{bookingId:int}")]
        [Authorize(Roles = "Traveler")]
        public async Task<IActionResult> Checkout(int bookingId, [FromQuery] string provider = "Paymob")
        {
            var userIdStr = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (!int.TryParse(userIdStr, out int userId)) return Unauthorized();

            var result = await _mediator.Send(new CreatePaymentSessionCommand
            {
                BookingId = bookingId,
                UserId = userId,
                Provider = provider
            });

            if (!result.IsSuccess)
                return BadRequest(new ProblemDetails
                {
                    Title = "Checkout Failed",
                    Detail = result.Error,
                    Status = StatusCodes.Status400BadRequest
                });

            return Ok(new { result.CheckoutUrl });
        }

        [HttpPost("webhook/{provider}")]
        [AllowAnonymous]
        public async Task<IActionResult> Webhook(string provider)
        {
            var webhookResult = HttpContext.Items["WebhookResult"] as WebhookResult;
            if (webhookResult == null) return Unauthorized();

            await _mediator.Send(new ProcessPaymentWebhookCommand
            {
                WebhookResult = webhookResult,
                ProviderName = provider
            });

            return Ok();
        }
    }
}
