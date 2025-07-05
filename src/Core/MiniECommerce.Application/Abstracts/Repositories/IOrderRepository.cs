using MiniECommerce.Domain.Entities;

namespace MiniECommerce.Application.Abstracts.Repositories;

public interface IOrderRepository : IRepository<Order>
{
    Task<Order?> GetOrderWithProductsAsync(Guid id);
    Task<List<Order>> GetAllWithProductsAsync();
}
