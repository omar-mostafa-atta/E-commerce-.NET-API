using E_commerce.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E_commerce.Services.Services.ProductService
{
	public interface IProductService
	{
		IEnumerable<Product> GetAll();
		Product GetById(int id);
		void Add(Product product);
		void Update(Product product);
		void Delete(int id);
		IEnumerable<Product> GetByCategoryId(int Categoryid);
	}
}
