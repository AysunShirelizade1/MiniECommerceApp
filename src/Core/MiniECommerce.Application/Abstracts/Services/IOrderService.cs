using MiniECommerce.Application.DTOs.Order;

namespace MiniECommerce.Application.Abstracts.Services;

public interface IOrderService
{
    Task<List<OrderListDto>> GetAllAsync();
    Task<OrderDetailDto?> GetByIdAsync(Guid id);
    Task<Guid> CreateAsync(OrderCreateDto dto, Guid buyerId);
    Task UpdateStatusAsync(OrderUpdateDto dto);
    Task DeleteAsync(Guid id);
}
