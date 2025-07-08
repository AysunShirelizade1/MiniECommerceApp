namespace MiniECommerce.Application.Abstracts.Repositories;
public interface IProductRepository : IRepository<Product>
{
    Task<List<Product>> GetAllWithCategoryAsync();
    Task<Product?> GetByIdWithIncludesAsync(Guid id);

}
