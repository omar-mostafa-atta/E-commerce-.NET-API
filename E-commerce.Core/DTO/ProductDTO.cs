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
		public string? Image { get; set; }
		public int CategoryId { get; set; } 
	}
}
