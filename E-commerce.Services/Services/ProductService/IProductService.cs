using E_commerce.Core.Models;
using Microsoft.AspNetCore.Http;
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
		void Add(Product product, IFormFile images);
		void Update(Product product, IFormFile? images);
		void Delete(int id);
		IEnumerable<Product> GetByCategoryId(int Categoryid);
	}
}
