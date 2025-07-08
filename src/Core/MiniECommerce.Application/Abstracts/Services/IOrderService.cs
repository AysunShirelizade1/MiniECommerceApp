using MiniECommerce.Application.DTOs.Order;
using MiniECommerce.Application.DTOs.OrderDto;

namespace MiniECommerce.Application.Abstracts.Services;

public interface IOrderService
{
    Task<List<OrderListDto>> GetAllAsync();
    Task<List<OrderListDto>> GetMySalesAsync(Guid userId);
    Task<OrderDetailDto?> GetByIdAsync(Guid id);
    Task<Guid> CreateAsync(OrderCreateDto dto, Guid buyerId);
    Task UpdateStatusAsync(OrderUpdateDto dto);
    Task DeleteAsync(Guid id);
    Task<List<OrderStatusHistoryDto>> GetStatusHistoryAsync(Guid orderId);
    Task CancelOrderAsync(Guid orderId);
    Task<List<OrderListDto>> GetOrdersByBuyerIdAsync(Guid buyerId);

}
