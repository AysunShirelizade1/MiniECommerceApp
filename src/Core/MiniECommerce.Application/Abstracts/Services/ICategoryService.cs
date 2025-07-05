using MiniECommerce.Application.DTOs.CategoryDto;

namespace MiniECommerce.Application.Abstractions.Services;
public interface ICategoryService
{
    Task<List<CategoryDto>> GetAllAsync();
    Task<CategoryDto?> GetByIdAsync(Guid id);
    Task<Guid> CreateAsync(CategoryCreateDto dto);
    Task UpdateAsync(Guid id, CategoryCreateDto dto);
    Task DeleteAsync(Guid id);
}
