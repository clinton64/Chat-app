using System.Linq.Expressions;

namespace Chat_app.Repository.IRepository;

public interface IRepository<T> where T : class
{
	Task<IEnumerable<TResult>> GetAll<TResult>(Expression<Func<T, bool>>? filer = null, string? includeProperties = null, Expression<Func<T, TResult>>? selector = null, Func<IQueryable<T>, IOrderedQueryable<T>>? orderBy = null);

	Task<T> GetFirstOrDefault(Expression<Func<T, bool>> filter, string? includeProperties = null, bool tracked = true);

	Task<bool> Exists(Expression<Func<T, bool>> filter);

	Task<T> Add(T entity);
	Task Remove(T entity);
	Task RemoveRange(IEnumerable<T> entities);
}
