using MiniECommerce.Application.DTOs.OrderProduct;

namespace MiniECommerce.Application.DTOs.Order;

public class OrderDto
{
    public Guid Id { get; set; }
    public string Status { get; set; } = null!;
    public Guid BuyerId { get; set; }
    public DateTime CreatedAt { get; set; }

    public List<OrderProductDto> Products { get; set; } = new();
}
