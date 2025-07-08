using MiniECommerce.Domain.Entities;

namespace MiniECommerce.Application.Abstracts.Repositories;

public interface ICategoryRepository : IRepository<Category>
{
    IQueryable<Category> GetAllIncludingSubCategories(bool includeDeleted = false);
    Task<Category?> GetByIdWithSubCategoriesAsync(Guid id, bool includeDeleted = false);
    Task<int> GetSubCategoriesCountAsync(Guid categoryId);
    Task<int> GetProductsCountByCategoryAsync(Guid categoryId);
}
