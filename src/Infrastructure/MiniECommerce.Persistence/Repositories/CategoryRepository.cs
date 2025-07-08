using Microsoft.EntityFrameworkCore;
using MiniECommerce.Application.Abstracts.Repositories;
using MiniECommerce.Domain.Entities;
using MiniECommerce.Persistence.Contexts;
using System.Linq;

namespace MiniECommerce.Persistence.Repositories;

public class CategoryRepository : Repository<Category>, ICategoryRepository
{
    private readonly MiniECommerceDbContext _context;

    public CategoryRepository(MiniECommerceDbContext context) : base(context)
    {
        _context = context;
    }

    public IQueryable<Category> GetAllIncludingSubCategories(bool includeDeleted = false)
    {
        var query = _context.Categories.AsQueryable();

        if (!includeDeleted)
        {
            query = query.Where(c => !c.IsDeleted);
        }

        return query
            .Where(c => c.ParentCategoryId == null)
            .Include(c => c.SubCategories.Where(sc => !sc.IsDeleted || includeDeleted));
    }

    public async Task<Category?> GetByIdWithSubCategoriesAsync(Guid id, bool includeDeleted = false)
    {
        return await _context.Categories
            .Include(c => c.SubCategories.Where(sc => !sc.IsDeleted || includeDeleted))
            .FirstOrDefaultAsync(c => c.Id == id && (!c.IsDeleted || includeDeleted));
    }

    public async Task<int> GetProductsCountByCategoryAsync(Guid categoryId)
    {
        return await _context.Products
            .CountAsync(p => p.CategoryId == categoryId && !p.IsDeleted);
    }


    public async Task<int> GetSubCategoriesCountAsync(Guid categoryId)
    {
        return await _context.Categories
            .CountAsync(c => c.ParentCategoryId == categoryId && !c.IsDeleted);
    }

}
