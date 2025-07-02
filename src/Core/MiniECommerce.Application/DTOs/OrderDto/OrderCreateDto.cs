using MiniECommerce.Application.DTOs.OrderProductItem;

namespace MiniECommerce.Application.DTOs.Order;

public class OrderCreateDto
{
    public List<OrderProductItemDto> Products { get; set; } = new();
}
