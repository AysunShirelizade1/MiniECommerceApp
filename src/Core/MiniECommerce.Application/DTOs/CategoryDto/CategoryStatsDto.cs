using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MiniECommerce.Application.DTOs.CategoryDto;

public class CategoryStatsDto
{
    public Guid CategoryId { get; set; }
    public int SubCategoriesCount { get; set; }
    public int ProductsCount { get; set; }
}
