using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace E_commerce.Core.Models
{
	public class Product : BaseEntity
	{
	 
		public string Name { get; set; }
		public string Description { get; set; }
		public string? Brand { get; set; }
		public string? Feedback { get; set; }
		public int Quantity { get; set; }
		public decimal Price { get; set; }
		public string? Img1 { get; set; }
		public string? Img2 { get; set; }
		public string? Img3 { get; set; }
		public string? Img4 { get; set; }
		public string? Img5 { get; set; }

		public int CategoryId { get; set; }
		[JsonIgnore]
		public Category Category { get; set; }

		public List<Order> Orders { get; set; }

	}
}
