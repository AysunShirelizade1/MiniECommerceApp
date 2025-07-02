using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MiniECommerce.Application.DTOs.CategoryDto;

public class CategoryCreateDto
{
    public string Name { get; set; } = null!;
    public Guid? ParentCategoryId { get; set; }
}