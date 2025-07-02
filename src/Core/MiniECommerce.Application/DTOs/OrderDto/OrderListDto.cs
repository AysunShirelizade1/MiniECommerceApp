namespace MiniECommerce.Application.DTOs.Order;

public class OrderListDto
{
    public Guid Id { get; set; }
    public string Status { get; set; } = null!;
    public DateTime CreatedAt { get; set; }
    public int ProductCount { get; set; }
}
