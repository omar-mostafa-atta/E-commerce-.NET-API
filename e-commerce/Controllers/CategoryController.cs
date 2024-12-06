using AutoMapper;
using E_commerce.API.DTO;
using E_commerce.Core.DTO;
using E_commerce.Core.Models;
using E_commerce.Services.Services.CategoryService;
using E_commerce.Services.Services.ProductService;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace e_commerce.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class CategoryController : ControllerBase
	{
		private readonly ICategoryService _categoryservice;
		private readonly IProductService _productservice;
		private readonly IMapper _mapper;
		public CategoryController(ICategoryService categoryService,IProductService productService , IMapper mapper)
        {
			_categoryservice = categoryService;
			_productservice = productService;
			_mapper = mapper;

		}
		[HttpPost("AddCategory")]
		public IActionResult Add(CategoryDTO CategoryDto)
		{
			if (!ModelState.IsValid) return BadRequest(ModelState);

			var Category = _mapper.Map<Category>(CategoryDto);


			_categoryservice.Add(Category);
			return CreatedAtAction(nameof(GetById), new { id = Category.Id }, Category);
		}
		[HttpPost("UpdateCategory/{Id}")]
		public IActionResult Update(int Id, CategoryDTO CategoryFromRequest)
		{
			var category = _categoryservice.GetById(Id);
			_mapper.Map(CategoryFromRequest, category);
			_categoryservice.Update(category);
			return Ok(category);

		}

		[HttpGet("GetByID/{Id}")]
		public IActionResult GetById(int Id)
		{
			var category = _categoryservice.GetById(Id);
			return Ok(category);

		}
		[HttpGet("GetAll")]
		public IActionResult GetAll()
		{
			var categories = _categoryservice.GetAll();
			return Ok(categories);

		}
		[HttpGet("GetAllProductsInTheCategory/{Id}")]
		public IActionResult GetAllProductsInTheCategory(int Id)
		{
			var AllProducts = _productservice.GetAll();
			var CategoryProducts = AllProducts.Where(x => x.CategoryId == Id);
			return Ok(CategoryProducts);

		}

	}
}
