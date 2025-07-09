namespace MiniECommerce.Application.DTOs.Product;

public class ProductDetailDto
{
    public Guid Id { get; set; }
    public string Title { get; set; } = null!;
    public string? Description { get; set; }
    public decimal Price { get; set; }
    public Guid CategoryId { get; set; }
    public string? CategoryName { get; set; }
    public List<string> ImageUrls { get; set; } = new();
    public Guid OwnerId { get; set; }
    public string? OwnerName { get; set; }
}
