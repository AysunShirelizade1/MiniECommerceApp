namespace MiniECommerce.Application.DTOs.Image;

public class CreateImageDto
{
    public string ImageUrl { get; set; } = null!;
    public bool IsMain { get; set; }
    public Guid ProductId { get; set; }
}
