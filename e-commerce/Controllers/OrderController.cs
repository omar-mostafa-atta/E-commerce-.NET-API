using AutoMapper;
using E_commerce.Core.DTO;
using E_commerce.Core.Models;
using E_commerce.Services.Services.OrderService;
using E_commerce.Services.Services.ProductService;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using Microsoft.AspNetCore.Identity;


namespace e_commerce.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class OrderController : ControllerBase
	{
		private readonly UserManager<ApplicationUser> userManager;
		private readonly IOrderService _orderService;
		private readonly IProductService _productService; // Assuming you have a service to fetch product details
		private readonly IMapper _mapper;
	 

		public OrderController(IOrderService orderService, IProductService productService, IMapper mapper, UserManager<ApplicationUser> _userManager)
		{
			_orderService = orderService;
			_productService = productService;
			_mapper = mapper;
			userManager = _userManager;
		}

		[HttpPost]
		[Authorize]
		public async Task< IActionResult> AddOrder( OrderDTO orderFromRequest)
		{
			var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

			if (userId == null)
			{
				return Unauthorized("User not authenticated.");
			}

			if (orderFromRequest == null)
			{
				return BadRequest("Order cannot be null, and UserId must be valid.");
			}

			if (!ModelState.IsValid)
			{
				return BadRequest(ModelState);
			}

			var orderProducts = new List<OrderProduct>();
			foreach (var item in orderFromRequest.ProductQuantities)
			{
				var product = await _productService.GetByIdAsync(item.ProductId); // Fetch product details
				if (product == null)
				{
					return BadRequest($"Product with ID {item.ProductId} not found.");
				}

				if (item.Quantity > product.Quantity)
				{
					return BadRequest($"No enough quantity for product {product.Name}. You Requested: {item.Quantity}, Available only: {product.Quantity}");
				}

				
				orderProducts.Add(new OrderProduct
				{
					ProductId = item.ProductId,
					Quantity = item.Quantity,
					Product =  product
				});

			
				product.Quantity -= item.Quantity;
				_productService.UpdateQuantityAsync(product.Id, product.Quantity);
			}

			var order = new Order
			{
				UserId = userId,
				TotalPrice = orderFromRequest.TotalPrice,
				OrderProducts = orderProducts
			};

			_orderService.AddAsync(order);
			return Ok("Order added successfully.");
		}
		[HttpGet]
		public IActionResult GetAll()
		{
		var orders=_orderService.GetAllAsync();
			return Ok(orders);
		}

	}
}
