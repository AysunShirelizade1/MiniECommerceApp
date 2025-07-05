using Microsoft.EntityFrameworkCore;
using MiniECommerce.Application.Abstracts.Repositories;
using MiniECommerce.Domain.Entities;
using MiniECommerce.Persistence.Contexts;

namespace MiniECommerce.Persistence.Repositories;

public class CategoryRepository : Repository<Category>, ICategoryRepository
{
    private readonly MiniECommerceDbContext _context;

    public CategoryRepository(MiniECommerceDbContext context) : base(context)
    {
        _context = context;
    }

    public async Task<List<Category>> GetAllWithSubCategoriesAsync()
    {
        return await _context.Categories
            .Where(c => c.ParentCategoryId == null)
            .Include(c => c.SubCategories)
            .ToListAsync();
    }

    public async Task<Category?> GetByIdWithSubCategoriesAsync(Guid id)
    {
        return await _context.Categories
            .Include(c => c.SubCategories)
            .FirstOrDefaultAsync(c => c.Id == id);
    }
}
