using AutoMapper;
using E_commerce.Core.DTO;
using E_commerce.Core.Models;
using E_commerce.Services.Services.OrderService;
using E_commerce.Services.Services.ProductService;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Linq;
using System.Security.Claims;

namespace e_commerce.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class OrderController : ControllerBase
	{
		private readonly IOrderService _orderService;
		private readonly IProductService _productService; // Assuming you have a service to fetch product details
		private readonly IMapper _mapper;

		public OrderController(IOrderService orderService, IProductService productService, IMapper mapper)
		{
			_orderService = orderService;
			_productService = productService;
			_mapper = mapper;
		}

		[HttpPost]
	    [Authorize(Roles ="Admin")]
		public IActionResult AddOrder(OrderDTO orderFromRequest)
		{
			var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value; // Extract UserId from token

			if (userId == null)
			{
				return Unauthorized("User not authenticated.");
			}

			int userIdInt = int.Parse(userId);
			if (orderFromRequest == null|| userIdInt== 0)
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
				var product = _productService.GetById(item.ProductId); // Fetch product details
				if (product == null)
				{
					return BadRequest($"Product with ID {item.ProductId} not found.");
				}

				if (item.Quantity > product.Quantity)
				{
					return BadRequest($"No enough quantity for product {product.Name}. You Requested: {item.Quantity}, Available only: {product.Quantity}");
				}

				// Add to order products list
				orderProducts.Add(new OrderProduct
				{
					ProductId = item.ProductId,
					Quantity = item.Quantity,
					Product = product
				});

				// Decrease product stock in the database
				product.Quantity -= item.Quantity;
				_productService.Update(product); // 34an a3ml Save the updated quantity
			}

			var order = new Order
			{
				UserId = userIdInt,
				TotalPrice = orderFromRequest.TotalPrice,
				OrderProducts = orderProducts
			};

			_orderService.Add(order);
			return Ok("Order added successfully.");
		}
	}
}
