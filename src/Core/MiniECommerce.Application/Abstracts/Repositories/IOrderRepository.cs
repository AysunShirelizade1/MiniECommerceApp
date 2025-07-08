using MiniECommerce.Domain.Entities;

namespace MiniECommerce.Application.Abstracts.Repositories;

public interface IOrderRepository : IRepository<Order>
{
    Task<Order?> GetOrderWithProductsAsync(Guid id);
    Task<List<Order>> GetAllWithProductsAsync();

    Task<List<OrderStatusHistory>> GetOrderStatusHistoryAsync(Guid orderId);
    Task AddOrderStatusHistoryAsync(OrderStatusHistory statusHistory);
    Task UpdateAsync(Order order);
}
