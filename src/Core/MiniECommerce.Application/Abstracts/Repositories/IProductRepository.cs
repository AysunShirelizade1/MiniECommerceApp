using MiniECommerceApp.Application.Repositories;
using MiniECommerceApp.Domain.Entities;

namespace MiniECommerceApp.Application.Abstract
{
    public interface IProductRepository : IRepository<Product>
    {
        Task<List<Product>> GetProductsWithIncludesAsync();
        Task<Product?> GetProductDetailAsync(Guid id);
    }
}
