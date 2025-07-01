namespace MiniECommerce.Domain.Entities;

public class OrderProduct : BaseEntity
{
    public Order Order { get; set; }

    public Guid ProductId { get; set; }
    public Product Product { get; set; }

    public int Quantity { get; set; }
    public decimal UnitPrice { get; set; }
}

