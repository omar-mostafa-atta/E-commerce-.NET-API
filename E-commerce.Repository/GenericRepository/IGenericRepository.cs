using System.Linq.Expressions;


namespace E_commerce.Repository.GenericRepository
{
	public interface IGenericRepository<TEntity> where TEntity : class
	{
		Task<IEnumerable<TEntity>> GetAllAsync();
		Task<TEntity> GetByIdAsync(int id);
		Task<TEntity> GetByIdAsync(string id);
		Task AddAsync(TEntity entity);
		Task UpdateAsync(TEntity entity);
		Task DeleteAsync(int id);
		Task DeleteAsync(string id);
		Task<IEnumerable<TEntity>> FindAsync(Expression<Func<TEntity, bool>> predicate);
		IQueryable<TEntity> GetAllQueryable();
	}
}
