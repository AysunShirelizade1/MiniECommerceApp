using Microsoft.EntityFrameworkCore;
using MiniECommerce.Application.Abstracts.Repositories;
using MiniECommerce.Persistence.Contexts;

namespace MiniECommerce.Persistence.Repositories;

public class ProductRepository : Repository<Product>, IProductRepository
{
    private readonly MiniECommerceDbContext _context;

    public ProductRepository(MiniECommerceDbContext context) : base(context)
    {
        _context = context;
    }

    public async Task<List<Product>> GetAllWithCategoryAsync()
    {
        return await _context.Products
            .Include(p => p.Category)
            .ToListAsync();
    }
    public async Task<Product?> GetByIdWithIncludesAsync(Guid id)
    {
        return await _context.Products
            .Include(p => p.Owner)
            .Include(p => p.Category)
            .Include(p => p.Images)
            .FirstOrDefaultAsync(p => p.Id == id);
    }
}
