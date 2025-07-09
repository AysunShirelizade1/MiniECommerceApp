namespace MiniECommerce.Application.DTOs.Product;

public class ProductListDto
{
    public Guid Id { get; set; }
    public string Title { get; set; } = null!;
    public string? Description { get; set; }
    public decimal Price { get; set; }
    public string CategoryName { get; set; } = null!;
    public List<string> ImageUrl { get; set; } = new();
    public Guid OwnerId { get; set; }
    public string? OwnerName { get; set; }
}
