using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace E_commerce.Repository.GenericRepository
{
	public interface IGenericRepository<TEntity> where TEntity : class
	{
		IEnumerable<TEntity> GetAll();
		TEntity GetById(int id);
		TEntity GetById(string id);
		void Add(TEntity entity);
		void Update(TEntity entity);
		void Delete(int id);
		void Delete(string id);
		IEnumerable<TEntity> Find(Expression<Func<TEntity, bool>> predicate);
		public IQueryable<TEntity> GetAllQueryable();

	}
}
