using E_commerce.Core.Models;
using E_commerce.Repository.GenericRepository;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace E_commerce.Services.Services.CategoryService
{
	public class CategoryService : ICategoryService
	{
		private readonly IGenericRepository<Category> _CategoryRepository;

		public CategoryService(IGenericRepository<Category> CategoryRepository)
		{
			_CategoryRepository = CategoryRepository;
		}

		public async Task<IEnumerable<Category>> GetAllAsync()
		{
			return await _CategoryRepository.GetAllAsync();  
		}
		public IQueryable<Category> GetAllQueryable()
		{

			return _CategoryRepository.GetAllQueryable();
		}

		public async Task<Category> GetByIdAsync(int id)
		{
			return await _CategoryRepository.GetByIdAsync(id); 
		}

		public async Task AddAsync(Category category)
		{
			await _CategoryRepository.AddAsync(category);  
		}

		public async Task UpdateAsync(Category category)
		{
			await _CategoryRepository.UpdateAsync(category);  
		}

		public async Task DeleteAsync(int id)
		{
			await _CategoryRepository.DeleteAsync(id);  
		}
	}
}
