using MiniECommerce.Application.DTOs.OrderProduct;

namespace MiniECommerce.Application.DTOs.Order;

public class OrderDetailDto
{
    public Guid Id { get; set; }
    public string Status { get; set; } = null!;
    public Guid BuyerId { get; set; }
    public string BuyerName { get; set; } = null!;
    public DateTime CreatedAt { get; set; }

    public List<OrderProductDto> Products { get; set; } = new();
}
