using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MiniECommerceApp.Domain.Entities;

namespace MiniECommerce.Application.DTOs.CategoryDto;

public class CategoryDto
{
    public Guid Id { get; set; }
    public string Name { get; set; } = null!;
    public Guid? ParentCategoryId { get; set; }
    public List<CategoryDto> SubCategories { get; set; } = new();

    public CategoryDto(Category category)
    {
        Id = category.Id;
        Name = category.Name;
        ParentCategoryId = category.ParentCategoryId;
        SubCategories = category.SubCategories?.Select(sc => new CategoryDto(sc)).ToList() ?? new();
    }
}
