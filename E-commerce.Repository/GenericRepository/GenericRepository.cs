using E_commerce.Repository.Data;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;


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

		public async Task<IEnumerable<TEntity>> GetAllAsync()
		{
		
			return await _dbSet.AsNoTracking().ToListAsync(); 
		}

		
		public IQueryable<TEntity> GetAllQueryable()
		{
			return _dbSet.AsQueryable(); 
		}

		public async Task<TEntity> GetByIdAsync(int id)
		{
			return await _dbSet.FindAsync(id); 
		}

		public async Task<TEntity> GetByIdAsync(string id)
		{
			return await _dbSet.FindAsync(id); 
		}

		public async Task AddAsync(TEntity entity)
		{
			await _dbSet.AddAsync(entity); 
			await _context.SaveChangesAsync(); 
		}

		public async Task<IEnumerable<TEntity>> FindAsync(Expression<Func<TEntity, bool>> predicate)
		{
			return await _dbSet.Where(predicate).ToListAsync(); 
		}

		public async Task UpdateAsync(TEntity entity)
		{
			_dbSet.Update(entity);
			await _context.SaveChangesAsync();
		}

		public async Task DeleteAsync(int id)
		{
			var entity = await _dbSet.FindAsync(id); 
			if (entity != null)
			{
				_dbSet.Remove(entity);
				await _context.SaveChangesAsync(); // Async version of SaveChanges
			}
		}

		public async Task DeleteAsync(string id)
		{
			var entity = await _dbSet.FindAsync(id); // Async version of Find
			if (entity != null)
			{
				_dbSet.Remove(entity);
				await _context.SaveChangesAsync(); // Async version of SaveChanges
			}
		}
	}
}
