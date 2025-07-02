using System.Linq.Expressions;
using MiniECommerceApp.Application.Repositories;
using MiniECommerceApp.Domain.Entities;

namespace MiniECommerceApp.Application.Abstract;


public interface IProductRepository : IRepository<Product>
{
    Task<List<Product>> GetProductsWithIncludesAsync();
    Task<Product?> GetProductDetailAsync(Guid id);

    IQueryable<Product> GetAllFiltered(
        Expression<Func<Product, bool>>? predicate = null,
        params Expression<Func<Product, object>>[] includeProperties);

    Task<Product?> GetByIdWithIncludesAsync(Guid id, params Expression<Func<Product, object>>[] includeProperties);
}
