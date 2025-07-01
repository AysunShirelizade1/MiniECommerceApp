using MiniECommerceApp.Domain.Entities;

public class Order : BaseEntity
{
    public string Status { get; set; } = null!;

    public Guid BuyerId { get; set; }
    public AppUser Buyer { get; set; } = null!;

    public ICollection<OrderProduct> OrderProducts { get; set; } = new List<OrderProduct>();
}
