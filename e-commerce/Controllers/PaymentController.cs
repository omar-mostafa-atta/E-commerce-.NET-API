using AutoMapper;
using E_commerce.Core.DTO;
using E_commerce.Core.Models;
using E_commerce.Services.Services.OrderService;
using E_commerce.Services.Services.PaymentService;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace E_commerce.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class PaymentController : ControllerBase
	{
		private readonly IOrderService _orderService;
		private readonly IPaymentService _paymentService;
		private readonly IMapper _mapper;

		public PaymentController(IOrderService orderService, IPaymentService paymentService, IMapper mapper)
		{
			_orderService = orderService;
			_paymentService = paymentService;
			_mapper = mapper;
		}

		[HttpPost]
		[Authorize]
		public async Task<IActionResult> ProcessPayment(PaymentDTO paymentFromRequest)
		{
			var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

			if (userId == null)
				return Unauthorized("User not authenticated.");

			if (!ModelState.IsValid)
				return BadRequest(ModelState);

			 
			var order = await _orderService.GetByIdAsync(paymentFromRequest.OrderId);
			if (order == null || order.UserId != userId)
				return BadRequest("Order not found or does not belong to the user.");

			 
			var payment = _mapper.Map<Payment>(paymentFromRequest);

			_paymentService.Add(payment);

			return Ok("Payment processed successfully.");
		}

		[HttpGet("{orderId}")]
		[Authorize]
		public IActionResult GetPaymentDetails(int orderId)
		{
			var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

			if (userId == null)
				return Unauthorized("User not authenticated.");

			var payment = _paymentService.GetByOrderId(orderId);

			if (payment == null || payment.UserId != userId)
				return BadRequest("Payment details not found or do not belong to the user.");

			return Ok(payment);
		}
	}
}
