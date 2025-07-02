namespace MiniECommerce.Application.DTOs.OrderProduct;

public class OrderProductDto
{
    public Guid ProductId { get; set; }
    public string ProductTitle { get; set; } = null!;
    public int Quantity { get; set; }
    public decimal UnitPrice { get; set; }
}
