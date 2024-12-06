using E_commerce.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E_commerce.Services.Services.CategoryService
{
	public interface ICategoryService
	{

		IEnumerable<Category> GetAll();
		Category GetById(int id);
		void Add(Category Category);
		void Update(Category Category);
		void Delete(int id);

	}
}
