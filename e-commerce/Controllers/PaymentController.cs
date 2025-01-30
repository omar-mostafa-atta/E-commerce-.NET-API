using Microsoft.AspNetCore.Mvc;
using E_commerce.Services.Services.PaymentService;
using E_commerce.Core.DTO;
using Stripe;
using Microsoft.AspNetCore.Authorization;

namespace e_commerce.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class PaymentController : ControllerBase
	{
		private readonly IPaymentService _paymentService;

		public PaymentController(IPaymentService paymentService)
		{
			_paymentService = paymentService;
		}

		[HttpPost("create-checkout-session")]
		[Authorize]
		public async Task<IActionResult> CreateCheckoutSession([FromBody] OrderDTO orderDTO)
		{
			try
			{
				var session = await _paymentService.CreateCheckoutSessionAsync(orderDTO.ProductQuantities);
				return Ok(new { sessionId = session.Id });
			}
			catch (StripeException e)
			{
				return BadRequest(new { error = e.StripeError.Message });
			}
		}
	}
}