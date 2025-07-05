using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using MiniECommerce.Application.Abstracts.Repositories;
using MiniECommerce.Domain.Entities;
using MiniECommerce.Persistence.Contexts;

namespace MiniECommerce.Persistence.Repositories;

public class Repository<T> : IRepository<T> where T : BaseEntity, new()
{
    private readonly MiniECommerceDbContext _context;
    private readonly DbSet<T> _dbSet;

    public Repository(MiniECommerceDbContext context)
    {
        _context = context;
        _dbSet = _context.Set<T>();
    }

    public async Task AddAsync(T entity)
    {
        await _dbSet.AddAsync(entity);
    }

    

    public IQueryable<T> GetAll(bool isTracking = false)
    {
        return isTracking ? _dbSet : _dbSet.AsNoTracking();
    }

    public IQueryable<T> GetAllFiltered(
        Expression<Func<T, bool>>? predicate = null,
        Expression<Func<T, object>>[]? include = null,
        Expression<Func<T, object>>[]? orderBy = null,
        bool isOrderByAsc = true,
        bool isTracking = false)
    {
        IQueryable<T> query = isTracking ? _dbSet : _dbSet.AsNoTracking();

        if (predicate != null)
            query = query.Where(predicate);

        if (include != null)
        {
            foreach (var includeProperty in include)
                query = query.Include(includeProperty);
        }

        if (orderBy != null)
        {
            foreach (var orderByProperty in orderBy)
            {
                query = isOrderByAsc ? query.OrderBy(orderByProperty) : query.OrderByDescending(orderByProperty);
            }
        }

        return query;
    }

    public IQueryable<T> GetByFiltered(
        Expression<Func<T, bool>>? predicate = null,
        Expression<Func<T, object>>[]? include = null,
        bool isTracking = false)
    {
        IQueryable<T> query = isTracking ? _dbSet : _dbSet.AsNoTracking();

        if (predicate != null)
            query = query.Where(predicate);

        if (include != null)
        {
            foreach (var includeProperty in include)
                query = query.Include(includeProperty);
        }

        return query;
    }

    public async Task<T?> GetByIdAsync(Guid id)
    {
        return await _dbSet.FindAsync(id);
    }

    public async Task SaveChangeAsync()
    {
        await _context.SaveChangesAsync();
    }
    public void Delete(T entity)
    {
        _dbSet.Remove(entity);
    }
    public void Update(T entity)
    {
        _dbSet.Update(entity);
    }
}
