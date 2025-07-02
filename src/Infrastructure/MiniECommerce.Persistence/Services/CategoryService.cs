using AutoMapper;
using MiniECommerce.Application.DTOs.CategoryDto;
using MiniECommerce.Application.Services.Repositories;
using MiniECommerceApp.Domain.Entities;
using MiniECommerceApp.Persistence.Repositories;
namespace MiniECommerceApp.Persistence.Services;
public class CategoryService : ICategoryService
{
    private readonly ICategoryRepository _categoryRepository;

    public CategoryService(ICategoryRepository categoryRepository)
    {
        _categoryRepository = categoryRepository;
    }


    public async Task<List<CategoryDto>> GetAllNestedAsync()
    {
        var categories = await _categoryRepository.GetAllAsync();

        // Nested struktura qurmaq üçün köməkçi metod:
        List<CategoryDto> BuildTree(IEnumerable<Category> categories, Guid? parentId = null)
        {
            return categories
                .Where(c => c.ParentCategoryId == parentId)
                .Select(c => new CategoryDto
                {
                    Id = c.Id,
                    Name = c.Name,
                    ParentCategoryId = c.ParentCategoryId,
                    SubCategories = BuildTree(categories, c.Id)
                }).ToList();
        }

        return BuildTree(categories);
    }

    public async Task CreateAsync(CategoryCreateDto dto)
    {
        var category = new Category
        {
            Id = Guid.NewGuid(),
            Name = dto.Name,
            ParentCategoryId = dto.ParentCategoryId,
            CreatedAt = DateTime.UtcNow
        };

        await _categoryRepository.AddAsync(category);
        await _categoryRepository.SaveChangeAsync();
    }

}
