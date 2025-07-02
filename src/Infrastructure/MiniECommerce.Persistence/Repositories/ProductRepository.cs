using Microsoft.EntityFrameworkCore;
using MiniECommerceApp.Application.Abstract;
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
    }
}
