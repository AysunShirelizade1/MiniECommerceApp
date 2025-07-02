namespace MiniECommerce.Application.DTOs.Image;

public class ImageDto
{
    public Guid Id { get; set; }
    public string ImageUrl { get; set; } = null!;
    public bool IsMain { get; set; }
}
