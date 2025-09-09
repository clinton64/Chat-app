using Chat_app.Data;
using Chat_app.Repository.IRepository;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace Chat_app.Repository;

public class Repository<T>: IRepository<T> where T : class
{
	private readonly AppDbContext _context;
	internal DbSet<T> dbSet;

	public Repository(AppDbContext context)
	{
		_context = context;
		this.dbSet = _context.Set<T>();
	}

	public async Task<IEnumerable<TResult>> GetAll<TResult>(
		Expression<Func<T, bool>>? filer = null,
		string? includeProperties = null,
		Expression<Func<T, TResult>>? selector = null,
		Func<IQueryable<T>, IOrderedQueryable<T>>? orderBy = null)
	{
		IQueryable<T> query = dbSet;

		if (filer != null)
		{
			query = query.Where(filer);
		}

		if (includeProperties != null)
		{
			foreach (var includeProp in includeProperties.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
			{
				query = query.Include(includeProp);
			}
		}

		if (orderBy != null)
		{
			query = orderBy(query);
		}

		if (selector != null)
			return await query.Select(selector).ToListAsync();	
		
		return await query.Cast<TResult>().ToListAsync();
	}
	public async Task<T> GetFirstOrDefault(Expression<Func<T, bool>> filter, string? includeProperties = null, bool tracked = true)
	{
		IQueryable<T> query = tracked ? dbSet : dbSet.AsNoTracking(); ;

		query = query.Where(filter);

		if (includeProperties != null)
		{
			foreach (var includeProp in includeProperties.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
			{
				query = query.Include(includeProp);
			}
		}
		return await query.FirstOrDefaultAsync() ?? throw new InvalidOperationException("Entity not found");
	}

	public async Task<bool> Exists(Expression<Func<T, bool>> filter)
	{
		return await dbSet.AnyAsync(filter);
	}

	public async Task<T> Add(T entity)
	{
		try
		{
			await dbSet.AddAsync(entity);
			await _context.SaveChangesAsync();
		}
		catch (Exception ex)
		{
			throw new Exception($"Error adding entity: {ex.Message}", ex);
		}
		return entity;

	}

	public async Task Remove(T entity)
	{
		try
		{
			dbSet.Remove(entity);
			await _context.SaveChangesAsync();
		}
		catch(Exception ex)
		{
			throw new Exception($"Error removing entity: {ex.Message}", ex);
		}
	}

	public async Task RemoveRange(IEnumerable<T> entities)
	{
		try
		{
			dbSet.RemoveRange(entities);
			await _context.SaveChangesAsync();
		}
		catch (Exception ex)
		{
			throw new Exception($"Error removing entities: {ex.Message}", ex);
		}
	}
}
