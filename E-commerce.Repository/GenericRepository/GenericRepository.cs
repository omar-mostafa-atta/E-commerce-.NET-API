using E_commerce.Repository.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace E_commerce.Repository.GenericRepository
{
	public class GenericRepository<TEntity> : IGenericRepository<TEntity> where TEntity : class
	{
		private readonly E_commerceContext _context;
		private readonly DbSet<TEntity> _dbSet;

		public GenericRepository(E_commerceContext context)
		{
			_context = context;
			_dbSet = _context.Set<TEntity>();
		}

		public IEnumerable<TEntity> GetAll()=>_dbSet.AsNoTracking().ToList();//AsNoTracking() makes the search faster ya a5oia
		public IQueryable<TEntity> GetAllQueryable()
		{
			return _dbSet; // Allows for further querying with Includes
		}

		public TEntity GetById(int id)=> _dbSet.Find(id);
		
		public TEntity GetById(string id)=> _dbSet.Find(id);
		

		public void Add(TEntity entity)
		{

			_dbSet.Add(entity);
			_context.SaveChanges();

		}
		public IEnumerable<TEntity> Find(Expression<Func<TEntity, bool>> predicate)=> _dbSet.Where(predicate);

		public void Update(TEntity entity)
		{
			_dbSet.Update(entity);
			_context.SaveChanges();
		}

		public void Delete(int id)
		{
			var entity = _dbSet.Find(id);
			if (entity != null)
			{
				_dbSet.Remove(entity);
				_context.SaveChanges();
			}
		}
		public void Delete(string id)
		{
			var entity = _dbSet.Find(id);
			if (entity != null)
			{
				_dbSet.Remove(entity);
				_context.SaveChanges();
			}
		}
	}
}
