using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MiniECommerce.Application.DTOs.ProductDto;

public class ProductDto
{
    public Guid Id { get; set; }
    public string Title { get; set; } = null!;
    public string Description { get; set; } = null!;
    public decimal Price { get; set; }
    public Guid OwnerId { get; set; }
    public Guid CategoryId { get; set; }
    public string CategoryName { get; set; } = null!;
    public List<string> ImageUrls { get; set; } = new();

    public ProductDto(Product product)
    {
        Id = product.Id;
        Title = product.Title;
        Description = product.Description;
        Price = product.Price;
        OwnerId = product.OwnerId;
        CategoryId = product.CategoryId;
        CategoryName = product.Category.Name;
        ImageUrls = product.Images.Select(i => i.ImageUrl).ToList();
    }
}