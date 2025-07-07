namespace MiniECommerce.Application.DTOs.ReviewDto;

public class ReviewCreateDto
{
    public string Comment { get; set; } = null!;
    public Guid ProductId { get; set; }  
}

