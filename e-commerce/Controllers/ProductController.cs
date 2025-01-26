using AutoMapper;
using E_commerce.API.DTO;
using E_commerce.Core.Models;
using E_commerce.Services.Services.CategoryService;
using E_commerce.Services.Services.ProductService;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;

namespace e_commerce.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class ProductController : ControllerBase
	{
		private readonly IProductService _productService;
		private readonly ICategoryService _categoryService;
		private readonly IMapper _mapper;

		public ProductController(IProductService productService, ICategoryService categoryService, IMapper mapper)
		{
			_productService = productService;
			_categoryService = categoryService;
			_mapper = mapper;
		}

		[HttpPost("AddProduct")]
		[Authorize(Roles = "Admin")]
		public async Task<IActionResult> Add([FromForm] ProductDto productDto)
		{
			if (!ModelState.IsValid) return BadRequest(ModelState);

			var product = _mapper.Map<Product>(productDto);

			await _productService.AddAsync(product, productDto.Images);

			return CreatedAtAction(nameof(GetById), new { id = product.Id }, product);
		}





		[HttpPost("Update/{id}")]
		public async Task<IActionResult> Update(int id, [FromForm] ProductDto productDto)
		{
			var productFromDb = await _productService.GetByIdAsync(id);
			if (productFromDb == null)
			{
				return NotFound("No Product With this Id");
			}

			_mapper.Map(productDto, productFromDb);
			await _productService.UpdateAsync(productFromDb, productDto.Images);
			return Ok();
		}



		[HttpGet("GetAll")]
		public async Task<IActionResult> GetAll(string? sortOrder, int pageNumber = 1, int pageSize = 10)
		{
			var query = _productService.GetAllAsync();


			switch (sortOrder)
			{
				case "A-Z_des":
					query = query.OrderByDescending(p => p.Name);
					break;
				case "price_asc":
					query = query.OrderBy(p => p.Price);
					break;
				case "price_des":
					query = query.OrderByDescending(p => p.Price);
					break;
				default:
					query = query.OrderBy(p => p.Name);
					break;
			}

			var totalCount = await query.CountAsync();
			var totalPages = (int)Math.Ceiling((decimal)totalCount / pageSize);

			if (pageNumber > totalPages) { pageNumber = totalPages; }


			var productsPerPage = await query
			.Skip((pageNumber - 1) * pageSize)
			.Take(pageSize)
			.ToListAsync();
			return Ok(productsPerPage);

		}





		[HttpGet("GetById/{id}")]
		public async Task<IActionResult> GetById(int id)
		{
			var product = await _productService.GetByIdAsync(id);
			if (product == null)
			{
				return NotFound("No Product With this Id");
			}
			return Ok(product);
		}

		[HttpGet("GetByCategoryId/{categoryId}")]
		public async Task<IActionResult> GetByCategoryId(int categoryId)
		{
			var products = await _productService.GetByCategoryIdAsync(categoryId); // Use async method
			return Ok(products);
		}
	}
}
