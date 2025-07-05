namespace MiniECommerce.Application.Abstracts.Repositories;
public interface IProductRepository : IRepository<Product>
{
    Task<List<Product>> GetAllWithCategoryAsync();
}
