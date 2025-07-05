namespace MiniECommerce.Application.DTOs.Product;

public class ProductCreateDto
{
    public string Title { get; set; } = null!;
    public string Description { get; set; } = null!;
    public decimal Price { get; set; }

    public Guid CategoryId { get; set; }
    public List<string>? ImageUrl { get; set; } 
}
