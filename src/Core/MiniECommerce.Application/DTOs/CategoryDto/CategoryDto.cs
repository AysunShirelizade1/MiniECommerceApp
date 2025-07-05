namespace MiniECommerce.Application.DTOs.CategoryDto;
public class CategoryDto
{
    public Guid Id { get; set; }
    public string Name { get; set; } = null!;
    public Guid? ParentCategoryId { get; set; }
    public List<CategoryDto> SubCategories { get; set; } = new();
}