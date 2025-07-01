namespace MiniECommerce.Domain.Entities;

public class Favorite : BaseEntity
{
    public Guid ProductId { get; set; }
    public Product Product { get; set; }

}
