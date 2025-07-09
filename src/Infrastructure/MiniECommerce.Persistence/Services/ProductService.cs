using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using MiniECommerce.Application.Abstracts.Repositories;
using MiniECommerce.Application.Abstracts.Services;
using MiniECommerce.Application.DTOs.Product;
using MiniECommerce.Domain.Entities;
namespace MiniECommerce.Persistence.Services;
public class ProductService : IProductService
{
    private readonly IRepository<Product> _productRepository;
    private readonly IRepository<Category> _categoryRepository;
    private readonly IRepository<Image> _imageRepository;

    public ProductService(
        IRepository<Product> productRepository,
        IRepository<Category> categoryRepository,
        IRepository<Image> imageRepository)
    {
        _productRepository = productRepository;
        _categoryRepository = categoryRepository;
        _imageRepository = imageRepository;
    }

    public async Task<ProductCreateDto> CreateAsync(ProductCreateDto dto, Guid userId)
    {
        var product = new Product
        {
            Title = dto.Title,
            Description = dto.Description,
            Price = dto.Price,
            CategoryId = dto.CategoryId,
            OwnerId = userId
        };

        await _productRepository.AddAsync(product);
        await _productRepository.SaveChangeAsync();

        if (dto.ImageUrl != null && dto.ImageUrl.Any())
        {
            foreach (var url in dto.ImageUrl)
            {
                var image = new Image
                {
                    ProductId = product.Id,
                    ImageUrl = url,
                    IsMain = false
                };
                await _imageRepository.AddAsync(image);
            }
            await _imageRepository.SaveChangeAsync();
        }

        return dto;
    }


    public async Task DeleteAsync(Guid id)
    {
        var product = await _productRepository.GetByIdAsync(id);
        if (product != null)
        {
            _productRepository.Delete(product);
            await _productRepository.SaveChangeAsync();
        }
    }

    public async Task<IEnumerable<ProductListDto>> GetAllAsync()
    {
        var products = _productRepository.GetAllFiltered(
            include: new Expression<Func<Product, object>>[]
            {
            p => p.Category,
            p => p.Images,
            p => p.Owner
            },
            isTracking: false);

        var list = await products.Select(p => new ProductListDto
        {
            Id = p.Id,
            Title = p.Title,
            Description = p.Description,
            Price = p.Price,
            CategoryName = p.Category.Name,
            ImageUrl = p.Images.Select(i => i.ImageUrl).ToList(),
            OwnerId = p.OwnerId,
            OwnerName = p.Owner != null ? p.Owner.UserName : "Unknown"
        }).ToListAsync();

        return list;
    }


    public async Task<ProductDetailDto?> GetByIdAsync(Guid id)
    {
        var productQuery = _productRepository.GetByFiltered(
            predicate: p => p.Id == id,
            include: new Expression<Func<Product, object>>[] { p => p.Category, p => p.Images, p => p.Owner },
            isTracking: false);

        var product = await productQuery.FirstOrDefaultAsync();
        if (product == null) return null;

        return new ProductDetailDto
        {
            Id = product.Id,
            Title = product.Title,
            Description = product.Description,
            Price = product.Price,
            CategoryId = product.CategoryId,
            CategoryName = product.Category.Name,
            ImageUrls = product.Images.Select(i => i.ImageUrl).ToList(),
            OwnerId = product.OwnerId,
            OwnerName = product.Owner?.UserName ?? "Unknown" 
        };

    }

    public async Task<List<ProductDetailDto>> GetProductsByUserIdAsync(Guid userId)
    {
        var query = _productRepository.GetByFiltered(
            predicate: p => p.OwnerId == userId,
            include: new Expression<Func<Product, object>>[] { p => p.Category, p => p.Images, p => p.Owner },
            isTracking: false);
        var productList = await query.ToListAsync();

        var result = productList.Select(p => new ProductDetailDto
        {
            Id = p.Id,
            Title = p.Title,
            Description = p.Description,
            Price = p.Price,
            CategoryId = p.CategoryId,
            CategoryName = p.Category?.Name ?? "N/A",
            ImageUrls = p.Images.Select(i => i.ImageUrl).ToList(),
            OwnerId = p.OwnerId,
            OwnerName = p.Owner != null ? p.Owner.UserName : "Unknown"
        }).ToList();

        return result;
    }




    public async Task UpdateAsync(Guid id, ProductUpdateDto dto)
    {
        var product = await _productRepository.GetByIdAsync(id);
        if (product == null) return;

        if (!string.IsNullOrEmpty(dto.Title))
            product.Title = dto.Title;

        if (!string.IsNullOrEmpty(dto.Description))
            product.Description = dto.Description;

        if (dto.Price.HasValue)
            product.Price = dto.Price.Value;

        if (dto.CategoryId.HasValue)
            product.CategoryId = dto.CategoryId.Value;

        _productRepository.Update(product);
        await _productRepository.SaveChangeAsync();
    }
}
