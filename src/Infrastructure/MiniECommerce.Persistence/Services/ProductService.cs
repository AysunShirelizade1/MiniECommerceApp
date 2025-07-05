using MiniECommerce.Application.Abstracts.Services;
using MiniECommerce.Application.DTOs.Product;
using MiniECommerceApp.Application.Repositories;
using MiniECommerceApp.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using MiniECommerceApp.Application.DTOs.Product;

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

    public async Task CreateAsync(ProductCreateDto dto)
    {
        var product = new Product
        {
            Title = dto.Title,
            Description = dto.Description,
            Price = dto.Price,
            CategoryId = dto.CategoryId,
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
            include: new Expression<Func<Product, object>>[] { p => p.Category, p => p.Images },
            isTracking: false);

        var list = await products.Select(p => new ProductListDto
        {
            Id = p.Id,
            Title = p.Title,
            Description = p.Description,
            Price = p.Price,
            CategoryName = p.Category.Name,
            ImageUrl = p.Images.Select(i => i.ImageUrl).ToList()
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
            OwnerId = product.OwnerId
        };
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
