using AutoMapper;
using E_commerce.Core.DTO;
using E_commerce.Core.Models;
using E_commerce.Services.Services.OrderService;
using E_commerce.Services.Services.ProductService;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using Microsoft.AspNetCore.Identity;
using E_commerce.Services.Services.PaymentService;

namespace e_commerce.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class OrderController : ControllerBase
	{
		private readonly UserManager<ApplicationUser> _userManager;
		private readonly IOrderService _orderService;
		private readonly IProductService _productService;
		private readonly IPaymentService _paymentService;
		private readonly IMapper _mapper;

		public OrderController(
			IOrderService orderService,
			IProductService productService,
			IMapper mapper,
			UserManager<ApplicationUser> userManager,
			IPaymentService paymentService)
		{
			_orderService = orderService;
			_productService = productService;
			_mapper = mapper;
			_userManager = userManager;
			_paymentService = paymentService;
		}

		[HttpPost("create-order")]
		[Authorize]
		public async Task<IActionResult> CreateOrder([FromBody] OrderDTO orderFromRequest)
		{
			var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

			if (userId == null)
			{
				return Unauthorized("User not authenticated.");
			}

			if (orderFromRequest == null || !ModelState.IsValid)
			{
				return BadRequest(ModelState);
			}

			// b3ml Create l Stripe checkout session
			var session = await _paymentService.CreateCheckoutSessionAsync(orderFromRequest.ProductQuantities);

			if (session == null)
			{
				return BadRequest("Failed to create Stripe checkout session.");
			}

	 
			var orderProducts = new List<OrderProduct>();
			foreach (var item in orderFromRequest.ProductQuantities)
			{
				var product = await _productService.GetByIdAsync(item.ProductId);
				if (product == null)
				{
					return BadRequest($"Product with ID {item.ProductId} not found.");
				}

				if (item.Quantity > product.Quantity)
				{
					return BadRequest($"Not enough quantity for product {product.Name}. You requested: {item.Quantity}, available only: {product.Quantity}");
				}

				orderProducts.Add(new OrderProduct
				{
					ProductId = item.ProductId,
					Quantity = item.Quantity,
					Product = product
				});


				product.Quantity -= item.Quantity;
				await _productService.UpdateQuantityAsync(product.Id, product.Quantity);
			}

		 
			var order = new Order
			{
				UserId = userId,
				TotalPrice = orderFromRequest.TotalPrice,
				OrderProducts = orderProducts,
				SessionId = session.Id  
			};

			await _orderService.AddAsync(order);

		 
			return Ok(new { sessionId = session.Id });
		}

		[HttpGet]
		public async Task<IActionResult> GetAll()
		{
			var orders = await _orderService.GetAllAsync();
			return Ok(orders);
		}
	}
}