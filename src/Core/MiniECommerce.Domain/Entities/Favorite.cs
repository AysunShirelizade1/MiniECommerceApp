namespace MiniECommerce.Domain.Entities;

public class Favorite : BaseEntity
{
    public Guid ProductId { get; set; }
    public Product Product { get; set; } = null!;

    public Guid AppUserId { get; set; }
    public AppUser AppUser { get; set; } = null!;
}
