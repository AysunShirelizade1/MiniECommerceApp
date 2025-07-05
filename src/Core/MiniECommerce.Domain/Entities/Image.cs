namespace MiniECommerce.Domain.Entities;

public class Image : BaseEntity
{
    public string ImageUrl { get; set; } = null!;
    public bool IsMain { get; set; }

    public Guid ProductId { get; set; }
    public Product Product { get; set; } = null!;
}
