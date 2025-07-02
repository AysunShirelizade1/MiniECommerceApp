namespace MiniECommerce.Application.DTOs.Order;

public class OrderUpdateDto
{
    public Guid OrderId { get; set; }
    public string Status { get; set; } = null!;
}
