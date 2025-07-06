using Microsoft.EntityFrameworkCore;
using MiniECommerce.Application.Abstracts.Repositories;
using MiniECommerce.Application.Abstracts.Services;
using MiniECommerce.Application.DTOs.CategoryDto;
using MiniECommerce.Domain.Entities;
namespace MiniECommerce.Persistence.Services;
public class CategoryService : ICategoryService
{
    private readonly IRepository<Category> _categoryRepository;

    public CategoryService(IRepository<Category> categoryRepository)
    {
        _categoryRepository = categoryRepository;
    }

    public async Task<List<CategoryDto>> GetAllAsync()
    {
        var categories = await _categoryRepository
            .GetAllFiltered(
                predicate: c => c.ParentCategoryId == null,
                include: new System.Linq.Expressions.Expression<Func<Category, object>>[] { c => c.SubCategories })
            .ToListAsync();

        return categories.Select(MapToDto).ToList();
    }

    public async Task<CategoryDto?> GetByIdAsync(Guid id)
    {
        var category = await _categoryRepository
            .GetByFiltered(
                predicate: c => c.Id == id,
                include: new System.Linq.Expressions.Expression<Func<Category, object>>[] { c => c.SubCategories })
            .FirstOrDefaultAsync();

        if (category == null)
            return null;

        return MapToDto(category);
    }

    public async Task<Guid> CreateAsync(CategoryCreateDto dto)
    {
        var category = new Category
        {
            Id = Guid.NewGuid(),
            Name = dto.Name,
            ParentCategoryId = dto.ParentCategoryId
        };

        await _categoryRepository.AddAsync(category);
        await _categoryRepository.SaveChangeAsync();

        return category.Id;
    }

    public async Task UpdateAsync(Guid id, CategoryCreateDto dto)
    {
        var category = await _categoryRepository.GetByIdAsync(id);
        if (category == null)
            throw new Exception("Category not found.");

        category.Name = dto.Name;
        category.ParentCategoryId = dto.ParentCategoryId;

        _categoryRepository.Update(category);
        await _categoryRepository.SaveChangeAsync();
    }

    public async Task DeleteAsync(Guid id)
    {
        var category = await _categoryRepository.GetByIdAsync(id);
        if (category == null)
            throw new Exception("Category not found.");

        if (category.SubCategories.Any())
            throw new Exception("Cannot delete category with subcategories.");

        _categoryRepository.Delete(category);
        await _categoryRepository.SaveChangeAsync();
    }

    private CategoryDto MapToDto(Category category)
    {
        return new CategoryDto
        {
            Id = category.Id,
            Name = category.Name,
            ParentCategoryId = category.ParentCategoryId,
            SubCategories = category.SubCategories?.Select(MapToDto).ToList() ?? new List<CategoryDto>()
        };
    }
}
