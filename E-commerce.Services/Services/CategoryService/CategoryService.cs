using E_commerce.Core.Models;
using E_commerce.Repository.GenericRepository;

namespace E_commerce.Services.Services.CategoryService
{
	public class CategoryService : ICategoryService
	{

		private readonly IGenericRepository<Category> _CategoryRepository;

		public CategoryService(IGenericRepository<Category> CategoryRepository)
		{
			_CategoryRepository = CategoryRepository;
		}

		public IEnumerable<Category> GetAll()
		{
			return _CategoryRepository.GetAll();
		}

		public Category GetById(int id)
		{
			return _CategoryRepository.GetById(id);
		}

		public void Add(Category Category)
		{
			_CategoryRepository.Add(Category);
		}

		public void Update(Category Category)
		{
			_CategoryRepository.Update(Category);
		}

		public void Delete(int id)
		{
			_CategoryRepository.Delete(id);
		}
	}

}
