namespace MiniECommerceApp.Application.DTOs.Product;

public class ProductListDto
{
    public Guid Id { get; set; }
    public string Name { get; set; } = null!;
    public string Description { get; set; } = null!;
    public decimal Price { get; set; }

    public string CategoryName { get; set; } = null!;
    public List<string> ImageUrl { get; set; } = new();
}
