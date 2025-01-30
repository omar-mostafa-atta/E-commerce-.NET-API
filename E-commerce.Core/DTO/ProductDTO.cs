using Microsoft.AspNetCore.Http;

namespace E_commerce.API.DTO
{
	public class ProductDto
	{
		public string Name { get; set; }
		public string Description { get; set; }
		public string? Brand { get; set; }
		public string? Feedback { get; set; }
		public int Quantity { get; set; }
		public decimal Price { get; set; }
		public int CategoryId { get; set; }
		public IFormFile[]? Images { get; set; }
		public string[]? DeleteImages { get; set; }

	}
}
