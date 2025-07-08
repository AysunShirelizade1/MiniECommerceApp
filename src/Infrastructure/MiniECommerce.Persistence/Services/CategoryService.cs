using Microsoft.EntityFrameworkCore;
using MiniECommerce.Application.Abstracts.Repositories;
using MiniECommerce.Application.Abstracts.Services;
using MiniECommerce.Application.DTOs.CategoryDto;
using MiniECommerce.Domain.Entities;
using MiniECommerce.Persistence.Contexts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MiniECommerce.Persistence.Services;

public class CategoryService : ICategoryService
{
    private readonly ICategoryRepository _categoryRepository;
    private readonly MiniECommerceDbContext _dbContext;

    public CategoryService(ICategoryRepository categoryRepository, MiniECommerceDbContext dbContext)
    {
        _categoryRepository = categoryRepository;
        _dbContext = dbContext;  // Burada DbContext-in instance-ını saxlayırıq
    }

    public async Task<(List<CategoryDto> Items, int TotalCount)> GetAllAsync(
        int pageNumber,
        int pageSize,
        string? sortBy,
        bool sortDesc,
        string? search)
    {
        var query = _categoryRepository.GetAllIncludingSubCategories();

        if (!string.IsNullOrWhiteSpace(search))
        {
            query = query.Where(c => c.Name.Contains(search));
        }

        query = (sortBy?.ToLower()) switch
        {
            "name" => sortDesc ? query.OrderByDescending(c => c.Name) : query.OrderBy(c => c.Name),
            "createdat" => sortDesc ? query.OrderByDescending(c => c.CreatedAt) : query.OrderBy(c => c.CreatedAt),
            _ => query.OrderBy(c => c.Name)
        };

        var totalCount = await query.CountAsync();

        var categories = await query
            .Skip((pageNumber - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync();

        var dtos = categories.Select(MapToDto).ToList();

        return (dtos, totalCount);
    }

    public async Task<CategoryStatsDto> GetStatsAsync(Guid categoryId)
    {
        var category = await _categoryRepository.GetByIdAsync(categoryId);
        if (category == null || category.IsDeleted)
            throw new Exception("Category not found.");

        var subCategoriesCount = await _categoryRepository.GetSubCategoriesCountAsync(categoryId);
        var productsCount = await _categoryRepository.GetProductsCountByCategoryAsync(categoryId);

        return new CategoryStatsDto
        {
            CategoryId = categoryId,
            SubCategoriesCount = subCategoriesCount,
            ProductsCount = productsCount
        };
    }
    public async Task<CategoryDto?> GetByIdAsync(Guid id)
    {
        var category = await _categoryRepository.GetByIdWithSubCategoriesAsync(id);

        if (category == null || category.IsDeleted)
            return null;

        return MapToDto(category);
    }

    public async Task<bool> BulkSoftDeleteAsync(List<Guid> ids)
    {
        var categories = await _categoryRepository.GetAll()
            .Where(c => ids.Contains(c.Id) && !c.IsDeleted)
            .ToListAsync();

        foreach (var category in categories)
        {
            var subCount = await _categoryRepository.GetSubCategoriesCountAsync(category.Id);
            if (subCount > 0) continue;

            category.IsDeleted = true;
            category.DeletedAt = DateTime.UtcNow;
        }

        await _dbContext.SaveChangesAsync();

        return true;
    }


    public async Task<List<CategoryDto>> GetCategoryTreeAsync()
    {
        var rootCategories = await _categoryRepository.GetAllIncludingSubCategories()
            .Where(c => c.ParentCategoryId == null && !c.IsDeleted)
            .ToListAsync();

        var dtoList = rootCategories.Select(MapToDto).ToList();
        return dtoList;
    }



    public async Task<Guid> CreateAsync(CategoryCreateDto dto)
    {
        var category = new Category
        {
            Id = Guid.NewGuid(),
            Name = dto.Name,
            ParentCategoryId = dto.ParentCategoryId,
            IsDeleted = false
        };

        await _categoryRepository.AddAsync(category);
        await _categoryRepository.SaveChangeAsync();

        return category.Id;
    }

    public async Task UpdateAsync(Guid id, CategoryCreateDto dto)
    {
        var category = await _categoryRepository.GetByIdAsync(id);
        if (category == null || category.IsDeleted)
            throw new Exception("Category not found.");

        category.Name = dto.Name;
        category.ParentCategoryId = dto.ParentCategoryId;

        _categoryRepository.Update(category);
        await _categoryRepository.SaveChangeAsync();
    }

    public async Task SoftDeleteAsync(Guid id)
    {
        var category = await _categoryRepository.GetByIdWithSubCategoriesAsync(id);
        if (category == null || category.IsDeleted)
            throw new Exception("Category not found.");

        if (category.SubCategories.Any(sc => !sc.IsDeleted))
            throw new Exception("Cannot delete category with active subcategories.");

        category.IsDeleted = true;
        category.DeletedAt = DateTime.UtcNow;

        _categoryRepository.Update(category);
        await _categoryRepository.SaveChangeAsync();
    }

    private CategoryDto MapToDto(Category category)
    {
        return new CategoryDto
        {
            Id = category.Id,
            Name = category.Name,
            ParentCategoryId = category.ParentCategoryId,
            SubCategories = category.SubCategories?
                .Where(sc => !sc.IsDeleted)
                .Select(MapToDto)
                .ToList() ?? new List<CategoryDto>()
        };
    }

    public async Task<(List<CategoryDto> items, int totalCount)> GetFilteredAsync(
    int pageNumber,
    int pageSize,
    string sortBy,
    bool sortDesc,
    string? search,
    bool flat)
    {
        IQueryable<Category> query = _dbContext.Categories.Where(c => !c.IsDeleted);

        if (!string.IsNullOrWhiteSpace(search))
        {
            query = query.Where(c => c.Name.Contains(search));
        }

        if (flat)
        {
            query = sortBy switch
            {
                "Name" => sortDesc ? query.OrderByDescending(c => c.Name) : query.OrderBy(c => c.Name),
                _ => query.OrderBy(c => c.Name)
            };

            var totalItems = await query.CountAsync();
            var categories = await query
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            var dtos = categories.Select(c => new CategoryDto
            {
                Id = c.Id,
                Name = c.Name,
                ParentCategoryId = c.ParentCategoryId,
                SubCategories = new List<CategoryDto>()
            }).ToList();

            return (dtos, totalItems);
        }
        else
        {
            query = query.Where(c => c.ParentCategoryId == null);

            query = sortBy switch
            {
                "Name" => sortDesc ? query.OrderByDescending(c => c.Name) : query.OrderBy(c => c.Name),
                _ => query.OrderBy(c => c.Name)
            };

            var totalItems = await query.CountAsync();

            var parentCategories = await query
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .Include(c => c.SubCategories.Where(sc => !sc.IsDeleted))
                .ToListAsync();

            var dtos = parentCategories.Select(MapToDto).ToList();

            return (dtos, totalItems);
        }
    }

}
