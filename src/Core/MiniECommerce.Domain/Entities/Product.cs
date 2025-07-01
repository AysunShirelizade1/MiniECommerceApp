using MiniECommerceApp.Domain.Entities;

public class Product : BaseEntity
{
    public string Title { get; set; } = null!;
    public string Description { get; set; } = null!;
    public decimal Price { get; set; }

    public Guid OwnerId { get; set; }
    public AppUser Owner { get; set; } = null!;

    public Guid CategoryId { get; set; }
    public Category Category { get; set; } = null!;

    public ICollection<Image> Images { get; set; } = new List<Image>();
    public ICollection<Favorite> Favorites { get; set; } = new List<Favorite>();
    public ICollection<OrderProduct> OrderProducts { get; set; } = new List<OrderProduct>();
    public ICollection<Review> Reviews { get; set; } = new List<Review>();
}
