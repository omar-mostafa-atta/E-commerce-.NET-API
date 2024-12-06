using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E_commerce.Core.Models
{
	public class Category:BaseEntity
	{
	 
		public string Name { get; set; }
		public string Description { get; set; }
		public string? Image { get; set; }

		public ICollection<Product>? Product { get; set; }
	}
}
