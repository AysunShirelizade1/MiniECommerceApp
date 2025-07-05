using MiniECommerce.Domain.Entities;

namespace MiniECommerce.Application.Abstracts.Repositories;

public interface ICategoryRepository : IRepository<Category>
{
    Task<List<Category>> GetAllWithSubCategoriesAsync();
    Task<Category?> GetByIdWithSubCategoriesAsync(Guid id);

}
