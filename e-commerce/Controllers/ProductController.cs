using AutoMapper;
using E_commerce.API.DTO;
using E_commerce.Core.Models;
using E_commerce.Services.Services.CategoryService;
using E_commerce.Services.Services.ProductService;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;

namespace e_commerce.Controllers
{


	[Route("api/[controller]")]
	[ApiController]

	public class ProductController : ControllerBase
	{
		private readonly IProductService _productService;
		private readonly ICategoryService _CategoryService;
		private readonly IMapper _mapper;
		public ProductController(IProductService productService, ICategoryService CategoryService, IMapper mapper)
		{
			_productService = productService;
			_CategoryService = CategoryService;
			_mapper = mapper;

		}


		[HttpPost("AddProduct")]
		[Authorize(Roles = "Admin")]
		public IActionResult Add(ProductDto productDto)
		{
			if (!ModelState.IsValid) return BadRequest(ModelState);

			var product = _mapper.Map<Product>(productDto);
		


			_productService.Add(product);
			return CreatedAtAction(nameof(GetById), new { id = product.Id }, product);
		}

		[HttpPost("Update/{Id}")]
		public IActionResult Update(int Id ,ProductDto productFromRequest)
		{
			var ProductFromDb = _productService.GetById(Id);
			if (ProductFromDb == null)
			{
				return NotFound("No Product With this Id");
			}

			#region before using automapper
			//ProductFromDb.Name = productFromRequest.Name;
			//ProductFromDb.Price = productFromRequest.Price;
			//ProductFromDb.Feedback = productFromRequest.Feedback;
			//ProductFromDb.Image = productFromRequest.Image;
			//ProductFromDb.CategoryId = productFromRequest.CategoryId;
			//ProductFromDb.Description = productFromRequest.Description;
			//ProductFromDb.Brand = productFromRequest.Brand;
			//ProductFromDb.Quantity = productFromRequest.Quantity;
			#endregion

			_mapper.Map(productFromRequest, ProductFromDb);

			_productService.Update(ProductFromDb);
			return Ok();

		}

		[HttpGet("GetAll")]
		public IActionResult GetAll()
		{
		 var Products= _productService.GetAll();

			return Ok(Products);

		}
		[HttpGet("GetById/{id}")]
		public IActionResult GetById(int id)
		{
			var Product= _productService.GetById(id);
			return Ok(Product);
		}


	}
}
