namespace MiniECommerce.Application.DTOs.Favorite;

public class FavoriteDto
{
    public Guid Id { get; set; }
    public Guid ProductId { get; set; }
    public string ProductTitle { get; set; } = null!;
    public string ProductImage { get; set; } = null!;
    public decimal Price { get; set; }
}
