using E_commerce.Core.Models;
using E_commerce.Repository.GenericRepository;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E_commerce.Services.Services.ProductService
{
	public class ProductService : IProductService
	{
		private readonly IGenericRepository<Product> _productRepository;
		private readonly IGenericRepository<Category> _categoryRepository;

		public ProductService(IGenericRepository<Product> productRepository, IGenericRepository<Category> categoryRepository)
		{
			_productRepository = productRepository;
			_categoryRepository = categoryRepository;
		}

		public IEnumerable<Product> GetAll()
		{
			return _productRepository.GetAll();
		}

		public Product GetById(int id)
		{
			return _productRepository.GetById(id);
		}
		public IEnumerable<Product> GetByCategoryId(int Categoryid)
		{
			return _productRepository.Find(x => x.CategoryId == Categoryid);
		}

		public void Add(Product product, IFormFile images)
		{
			var category = _categoryRepository.GetById(product.CategoryId);
			if (category == null)
			{
				throw new ArgumentException("Invalid CategoryId");
			}
			_productRepository.Add(product);
		}

		public void Update(Product product, IFormFile images)
		{
			_productRepository.Update(product);
		}

		public void Delete(int id)
		{
			_productRepository.Delete(id);
		}
	}
}
