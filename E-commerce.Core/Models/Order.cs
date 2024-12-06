using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace E_commerce.Core.Models
{
	public class Order : BaseEntity
	{
		public decimal TotalPrice { get; set; }
		public int UserId { get; set; }
		public ApplicationUser User { get; set; }

		[NotMapped]
		public List<OrderProduct> OrderProducts { get; set; } = new List<OrderProduct>(); 
	}

	[NotMapped]
	public class OrderProduct
	{
		
		public int ProductId { get; set; }
		public Product Product { get; set; }
		public int Quantity { get; set; } 
		
		                                       
	}
}
