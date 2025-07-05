namespace MiniECommerce.Application.DTOs.CategoryDto;

public class CategoryCreateDto
{
    public string Name { get; set; } = null!;
    public Guid? ParentCategoryId { get; set; }
}