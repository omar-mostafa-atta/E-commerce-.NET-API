using E_commerce.Core.Models;
using Microsoft.AspNetCore.Http;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace E_commerce.Services.Services.ProductService
{
	public interface IProductService
	{
		IQueryable<Product> GetAllAsync();
		Task<Product> GetByIdAsync(int id);
	
		Task AddAsync(Product product, IFormFile[]? images);
		Task UpdateAsync(Product product, IFormFile[]? images);
		Task UpdateQuantityAsync(int productId, int newQuantity);
		Task DeleteAsync(int id);
		Task<IEnumerable<Product>> GetByCategoryIdAsync(int categoryId);
	}
}
