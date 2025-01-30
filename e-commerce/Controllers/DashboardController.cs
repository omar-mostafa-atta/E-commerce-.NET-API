using AutoMapper;
using E_commerce.Core.Models;
using E_commerce.Services.Services.CategoryService;
using E_commerce.Services.Services.OrderService;
using E_commerce.Services.Services.PaymentService;
using E_commerce.Services.Services.ProductService;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace e_commerce.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	[Authorize(Roles = "Admin")]
	public class DashboardController : ControllerBase
	{

		private readonly Microsoft.AspNetCore.Identity.UserManager<ApplicationUser> _userManager;
		private readonly IOrderService _orderService;
		private readonly IProductService _productService;
		private readonly IPaymentService _paymentService;
		private readonly ICategoryService _categoryService;
		public DashboardController(IOrderService orderService,IProductService productService,Microsoft.AspNetCore.Identity.UserManager<ApplicationUser> userManager,IPaymentService paymentService, ICategoryService categoryService)
		{
			_orderService = orderService;
			_productService = productService;
			_userManager = userManager;
			_paymentService = paymentService;
			_categoryService = categoryService;
		}

		[HttpGet("GetProductsNumber")]
		[Authorize(Roles = "Admin")]
		public IActionResult GetNumberOfProducts()
		{
			var products=_productService.GetAllAsync();
			var num= products.Count();
			return Ok(num);	

		}
		[HttpGet("GetCategoriesNumber")]
		[Authorize(Roles = "Admin")]
		public IActionResult GetCategoriesNumber()
		{

			
			var categories=_categoryService.GetAllQueryable();
			var num= categories.Count();
			return Ok(num);	

		}
		[HttpGet("GetUsersNumber")]
		[Authorize(Roles = "Admin")]
		public async Task<IActionResult> GetUsersNumber()
		{
			var userCount = await _userManager.Users.CountAsync();
			return Ok(new { UserCount = userCount });
		}

		[HttpGet("GetOrderNumber")]
		[Authorize(Roles = "Admin")]
		public IActionResult GetOrderNumber()
		{
			var order = _orderService.GetAllQueryable();
			var num=order.Result.Count();
			return Ok(num);
		}

		[HttpDelete("Delete User/{id}")]
		[Authorize(Roles = "Admin")]
		public async Task<IActionResult> DeleteUser(string id)
		{
			var user = await _userManager.FindByIdAsync(id);

			if (user == null)
			{
				return NotFound("User not found.");
			}

			var result = await _userManager.DeleteAsync(user);

			if (result.Succeeded)
			{
				return Ok($"User with ID {id} has been deleted successfully.");
			}

			return BadRequest("Failed to delete the user.");

		}


	}
}
