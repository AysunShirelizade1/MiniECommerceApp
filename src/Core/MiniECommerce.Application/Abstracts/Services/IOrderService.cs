using MiniECommerce.Application.Common;
using MiniECommerce.Application.DTOs.Order;
using MiniECommerce.Application.Services.Repositories;

namespace MiniECommerce.Application.Services;

public class IOrderService : IOrderRepository
{
    public Task<ServiceResult<OrderDto>> CreateOrderAsync(OrderCreateDto dto, Guid buyerId)
    {
        throw new NotImplementedException();
    }

    public Task<ServiceResult<OrderDto>> GetOrderByIdAsync(Guid orderId, Guid userId)
    {
        throw new NotImplementedException();
    }

    public Task<IEnumerable<OrderDto>> GetOrdersByBuyerIdAsync(Guid buyerId)
    {
        throw new NotImplementedException();
    }

    public Task<IEnumerable<OrderDto>> GetSalesBySellerIdAsync(Guid sellerId)
    {
        throw new NotImplementedException();
    }
}
