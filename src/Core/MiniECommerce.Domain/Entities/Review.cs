namespace MiniECommerce.Domain.Entities;

public class Review : BaseEntity
{
    public string Comment { get; set; } = null!;

    public Guid ProductId { get; set; }
    public Product Product { get; set; } = null!;
    public Guid UserId { get; set; }
    public AppUser User { get; set; } = null!;

}
