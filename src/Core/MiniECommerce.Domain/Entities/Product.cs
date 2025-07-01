namespace MiniECommerce.Domain.Entities;

public class Product : BaseEntity
{
    public string Title { get; set; }
    public string Description { get; set; }

    public decimal Price { get; set; }

    public Guid OwnerId { get; set; }

    public Guid CategoryId { get; set; }
    public Category Category { get; set; }

    public ICollection<Image> Images { get; set; }
    public ICollection<Favorite> Favorites { get; set; }
    public ICollection<OrderProduct> OrderProducts { get; set; }
    public ICollection<Review> Reviews { get; set; }
    public object ImageUrl { get; set; }
}
