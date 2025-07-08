using MiniECommerce.Application.DTOs.CategoryDto;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MiniECommerce.Application.Abstracts.Services;

public interface ICategoryService
{
    Task<(List<CategoryDto> Items, int TotalCount)> GetAllAsync(
        int pageNumber,
        int pageSize,
        string? sortBy,
        bool sortDesc,
        string? search);

    Task<CategoryDto?> GetByIdAsync(Guid id);
    Task<Guid> CreateAsync(CategoryCreateDto dto);
    Task UpdateAsync(Guid id, CategoryCreateDto dto);
    Task SoftDeleteAsync(Guid id);

    Task<CategoryStatsDto> GetStatsAsync(Guid categoryId);

    // Yeni əlavə etdiklərimiz:
    Task<bool> BulkSoftDeleteAsync(List<Guid> ids);

    Task<List<CategoryDto>> GetCategoryTreeAsync();
    Task<(List<CategoryDto> items, int totalCount)> GetFilteredAsync(int pageNumber, int pageSize, string sortBy, bool sortDesc, string? search, bool flat);

}
