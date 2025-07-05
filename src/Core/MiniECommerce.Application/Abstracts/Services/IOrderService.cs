using MiniECommerce.Application.DTOs.Order;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MiniECommerce.Application.Abstracts.Services;

public interface IOrderService
{
    Task<IEnumerable<OrderListDto>> GetAllAsync();
    Task<OrderDetailDto?> GetByIdAsync(Guid id);
    Task CreateAsync(OrderCreateDto dto);
    Task UpdateAsync(OrderUpdateDto dto);
    Task DeleteAsync(Guid id);
}
