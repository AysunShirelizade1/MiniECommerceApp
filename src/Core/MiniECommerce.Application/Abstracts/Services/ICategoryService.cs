using MiniECommerce.Application.DTOs.CategoryDto;

public interface ICategoryService
{
    Task<List<CategoryDto>> GetAllNestedAsync();
    Task CreateAsync(CategoryCreateDto dto);

}
