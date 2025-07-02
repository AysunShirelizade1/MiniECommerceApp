using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using MiniECommerceApp.Application.Abstract;
using MiniECommerceApp.Domain.Entities;
using MiniECommerceApp.Persistence.Contexts;

namespace MiniECommerceApp.Persistence.Repositories
{
    public class ProductRepository : Repository<Product>, IProductRepository
    {
        private readonly MiniECommerceDbContext _context;

        public ProductRepository(MiniECommerceDbContext context) : base(context)
        {
            _context = context;
        }

        public async Task<List<Product>> GetProductsWithIncludesAsync()
        {
            return await _context.Products
                .Include(p => p.Category)
                .Include(p => p.Images)
                .ToListAsync();
        }

        public async Task<Product?> GetProductDetailAsync(Guid id)
        {
            return await _context.Products
                .Include(p => p.Category)
                .Include(p => p.Images)
                .FirstOrDefaultAsync(p => p.Id == id);
        }

        public IQueryable<Product> GetAllFiltered(Expression<Func<Product, bool>>? predicate = null, params Expression<Func<Product, object>>[] includeProperties)
        {
            IQueryable<Product> query = _context.Products;

            foreach (var includeProperty in includeProperties)
            {
                query = query.Include(includeProperty);
            }

            if (predicate != null)
            {
                query = query.Where(predicate);
            }

            return query;
        }

        public async Task<Product?> GetByIdWithIncludesAsync(Guid id, params Expression<Func<Product, object>>[] includeProperties)
        {
            IQueryable<Product> query = _context.Products;

            foreach (var includeProperty in includeProperties)
            {
                query = query.Include(includeProperty);
            }

            return await query.FirstOrDefaultAsync(p => p.Id == id);
        }
    }
}
