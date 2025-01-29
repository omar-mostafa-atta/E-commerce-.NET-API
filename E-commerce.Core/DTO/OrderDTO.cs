using System.Collections.Generic;

namespace E_commerce.Core.DTO
{
	public class OrderDTO
	{
		public decimal TotalPrice { get; set; }
		public string Currency { get; set; } = "usd"; 
		public List<ProductQuantityDTO> ProductQuantities { get; set; }
	}

	public class ProductQuantityDTO
	{
		public int ProductId { get; set; }
		public string ProductName { get; set; }
		public int Quantity { get; set; }
		public decimal Price { get; set; } 
	}
}
