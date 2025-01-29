using AutoMapper;
using E_commerce.Core.Models;
using E_commerce.Services.Services.CategoryService;
using E_commerce.Services.Services.OrderService;
using E_commerce.Services.Services.PaymentService;
using E_commerce.Services.Services.ProductService;
 
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace e_commerce.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	
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
		public IActionResult GetNumberOfProducts()
		{
			var products=_productService.GetAllAsync();
			var num= products.Count();
			return Ok(num);	

		}
		[HttpGet("GetCategoriesNumber")]
		public IActionResult GetCategoriesNumber()
		{

			
			var categories=_categoryService.GetAllQueryable();
			var num= categories.Count();
			return Ok(num);	

		}
		[HttpGet("GetUserNumber")]
		public async Task<IActionResult> GetUserNumber()
		{
			var userCount = await _userManager.Users.CountAsync();
			return Ok(new { UserCount = userCount });
		}

		[HttpGet("GetOrderNumber")]
		public IActionResult GetOrderNumber()
		{
			var order = _orderService.GetAllQueryable();
			var num=order.Result.Count();
			return Ok(num);
		}


	}
}
