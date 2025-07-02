using System.Linq.Expressions;
using MiniECommerceApp.Domain.Entities;
using MiniECommerceApp.Application.Repositories;
using MiniECommerceApp.Persistence.Contexts;
using Microsoft.EntityFrameworkCore;

namespace MiniECommerceApp.Persistence.Repositories;

public class Repository<T> : IRepository<T> where T : BaseEntity, new()
{
    private readonly MiniECommerceDbContext _context;
    private readonly DbSet<T> _table;

    public Repository(MiniECommerceDbContext context)
    {
        _context = context;
        _table = _context.Set<T>();
    }

    public async Task AddAsync(T entity)
    {
        entity.CreatedAt = DateTime.UtcNow;
        await _table.AddAsync(entity);
    }

    public void Update(T entity)
    {
        entity.UpdatedAt = DateTime.UtcNow;
        _table.Update(entity);
    }

    public void Delete(T entity)
    {
        _table.Remove(entity);
    }

    public async Task<T?> GetByIdAsync(Guid id)
    {
        return await _table.FindAsync(id);
    }

    public IQueryable<T> GetByFiltered(
        Expression<Func<T, bool>>? predicate = null,
        Expression<Func<T, object>>[]? include = null,
        bool isTracking = false)
    {
        IQueryable<T> query = _table;

        if (predicate != null)
            query = query.Where(predicate);

        if (include != null)
        {
            foreach (var inc in include)
                query = query.Include(inc);
        }

        return isTracking ? query : query.AsNoTracking();
    }

    public IQueryable<T> GetAll(bool isTracking = false)
    {
        return isTracking ? _table : _table.AsNoTracking();
    }

    public IQueryable<T> GetAllFiltered(
        Expression<Func<T, bool>>? predicate = null,
        Expression<Func<T, object>>[]? include = null,
        Expression<Func<T, object>>[]? orderBy = null,
        bool isOrderByAsc = true,
        bool isTracking = false)
    {
        IQueryable<T> query = _table;

        if (predicate != null)
            query = query.Where(predicate);

        if (include != null)
        {
            foreach (var inc in include)
                query = query.Include(inc);
        }

        if (orderBy != null && orderBy.Length > 0)
        {
            IOrderedQueryable<T> orderedQuery = isOrderByAsc
                ? query.OrderBy(orderBy[0])
                : query.OrderByDescending(orderBy[0]);

            for (int i = 1; i < orderBy.Length; i++)
            {
                orderedQuery = isOrderByAsc
                    ? orderedQuery.ThenBy(orderBy[i])
                    : orderedQuery.ThenByDescending(orderBy[i]);
            }

            query = orderedQuery;
        }

        return isTracking ? query : query.AsNoTracking();
    }

    public async Task SaveChangeAsync()
    {
        await _context.SaveChangesAsync();
    }
}