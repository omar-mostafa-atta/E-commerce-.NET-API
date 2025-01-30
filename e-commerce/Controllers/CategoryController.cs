using AutoMapper;
using E_commerce.API.DTO;
using E_commerce.Core.DTO;
using E_commerce.Core.Models;
using E_commerce.Services.Services.CategoryService;
using E_commerce.Services.Services.ProductService;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;

namespace e_commerce.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class CategoryController : ControllerBase
	{
		private readonly ICategoryService _categoryService;
		private readonly IProductService _productService;
		private readonly IMapper _mapper;

		public CategoryController(ICategoryService categoryService, IProductService productService, IMapper mapper)
		{
			_categoryService = categoryService;
			_productService = productService;
			_mapper = mapper;
		}

		[HttpPost("AddCategory")]
		[Authorize(Roles = "Admin")]
		public async Task<IActionResult> Add(CategoryDTO categoryDto)
		{
			if (!ModelState.IsValid)
				return BadRequest(ModelState);

			var category = _mapper.Map<Category>(categoryDto);

			await _categoryService.AddAsync(category); 
			return CreatedAtAction(nameof(GetById), new { id = category.Id }, category);
		}

		[HttpPut("UpdateCategory/{Id}")]
		[Authorize(Roles = "Admin")]
		public async Task<IActionResult> Update(int id, CategoryDTO categoryFromRequest)
		{
			var category = await _categoryService.GetByIdAsync(id); 
			if (category == null)
				return NotFound(); 

			_mapper.Map(categoryFromRequest, category);
			await _categoryService.UpdateAsync(category); 
			return Ok(category);
		}

		[HttpGet("GetByID/{Id}")]
		public async Task<IActionResult> GetById(int id)
		{
			var category = await _categoryService.GetByIdAsync(id); 
			if (category == null)
				return NotFound(); 

			return Ok(category);
		}

		[HttpGet("GetAll")]
		public async Task<IActionResult> GetAll(string? sortOrder, int pageNumber = 1, int pageSize = 10)
		{
			
			var query = _categoryService.GetAllQueryable();

			switch (sortOrder)
			{
				case "A-Z_des":
					query = query.OrderByDescending(p => p.Name);
					break;
	
				default:
					query = query.OrderBy(p => p.Name);
					break;
			}

			var totalCount = await query.CountAsync();
			var totalPages = (int)Math.Ceiling((decimal)totalCount / pageSize);

			if (pageNumber > totalPages) { pageNumber = totalPages; }


			var categoriesPerPage = await query
			.Skip((pageNumber - 1) * pageSize)
			.Take(pageSize)
			.ToListAsync();
			return Ok(categoriesPerPage);
		}

		[HttpGet("GetAllProductsInTheCategory/{Id}")]
		public async Task<IActionResult> GetAllProductsInTheCategory(int id)
		{
			var allProducts = await _productService.GetByCategoryIdAsync(id); 
		
			return Ok(allProducts);
		}

		[HttpDelete]
		public async Task<IActionResult> Delete(int id)
		{
			var category = await _categoryService.GetByIdAsync(id);
			if (category == null)
				return NotFound("No Category With this Id");

			_categoryService.DeleteAsync(id);
			return Ok("Category deleted");

		}

	}
}
