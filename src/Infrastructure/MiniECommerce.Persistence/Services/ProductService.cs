using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using MiniECommerce.Application.DTOs.Product;
using MiniECommerce.Application.Services;
using MiniECommerceApp.Application.Abstract;
using MiniECommerceApp.Application.DTOs.Product;
using MiniECommerceApp.Domain.Entities;

namespace MiniECommerceApp.Persistence.Services
{
    public class ProductService : IProductService
    {
        private readonly IProductRepository _productRepository;

        public ProductService(IProductRepository productRepository)
        {
            _productRepository = productRepository;
        }

        public async Task<List<ProductListDto>> GetAllFilteredAsync(Guid? categoryId = null, decimal? minPrice = null, decimal? maxPrice = null, string? search = null)
        {
            Expression<Func<Product, bool>> predicate = p =>
                (!categoryId.HasValue || p.CategoryId == categoryId) &&
                (!minPrice.HasValue || p.Price >= minPrice) &&
                (!maxPrice.HasValue || p.Price <= maxPrice) &&
                (string.IsNullOrEmpty(search) || p.Title.Contains(search, StringComparison.OrdinalIgnoreCase));

            var productsQuery = _productRepository.GetAllFiltered(
                predicate: predicate,
                include: new Expression<Func<Product, object>>[] { p => p.Category, p => p.Images }
            );

            var products = await productsQuery.ToListAsync();

            var productListDtos = products.Select(p => new ProductListDto
            {
                Id = p.Id,
                Name = p.Title,
                Description = p.Description ?? string.Empty,
                Price = p.Price,
                CategoryName = p.Category?.Name ?? string.Empty,
                ImageUrl = p.Images?.Select(i => i.ImageUrl).ToList() ?? new List<string>()
            }).ToList();

            return productListDtos;
        }

        public async Task<ProductDetailDto?> GetByIdAsync(Guid id)
        {
            var product = await _productRepository.GetByIdWithIncludesAsync(id, new Expression<Func<Product, object>>[] { p => p.Category, p => p.Images });
            if (product == null) return null;

            return new ProductDetailDto
            {
                Id = product.Id,
                Title = product.Title,
                Description = product.Description,
                Price = product.Price,
                CategoryId = product.CategoryId,
                CategoryName = product.Category?.Name,
                ImageUrls = product.Images?.Select(i => i.ImageUrl).ToList() ?? new List<string>(),
                OwnerId = product.OwnerId
            };
        }

        public async Task<Guid> CreateAsync(ProductCreateDto dto, Guid ownerId)
        {
            var product = new Product
            {
                Id = Guid.NewGuid(),
                Title = dto.Name,
                Description = dto.Description,
                Price = dto.Price,
                CategoryId = dto.CategoryId,
                OwnerId = ownerId,
                CreatedAt = DateTime.UtcNow
            };

            await _productRepository.AddAsync(product);
            await _productRepository.SaveChangeAsync();

            return product.Id;
        }


        public async Task<bool> UpdateAsync(Guid id, ProductUpdateDto dto, Guid ownerId)
        {
            var product = await _productRepository.GetByIdAsync(id);
            if (product == null || product.OwnerId != ownerId)
                return false;

            if (dto.Name != null) product.Title = dto.Name;
            if (dto.Description != null) product.Description = dto.Description;
            if (dto.Price.HasValue) product.Price = dto.Price.Value;
            if (dto.CategoryId.HasValue) product.CategoryId = dto.CategoryId.Value;

            product.UpdatedAt = DateTime.UtcNow;

            _productRepository.Update(product);
            await _productRepository.SaveChangeAsync();
            return true;
        }

        public async Task<bool> DeleteAsync(Guid id, Guid ownerId)
        {
            var product = await _productRepository.GetByIdAsync(id);
            if (product == null || product.OwnerId != ownerId)
                return false;

            _productRepository.Delete(product);
            await _productRepository.SaveChangeAsync();
            return true;
        }

        public async Task<List<ProductListDto>> GetProductsByOwnerAsync(Guid ownerId)
        {
            var productsQuery = _productRepository.GetAllFiltered(
                predicate: p => p.OwnerId == ownerId,
                include: new Expression<Func<Product, object>>[] { p => p.Category, p => p.Images }
            );

            var products = await productsQuery.ToListAsync();

            var productListDtos = products.Select(p => new ProductListDto
            {
                Id = p.Id,
                Name = p.Title,
                Description = p.Description ?? string.Empty,
                Price = p.Price,
                CategoryName = p.Category?.Name ?? string.Empty,
                ImageUrl = p.Images?.Select(i => i.ImageUrl).ToList() ?? new List<string>()
            }).ToList();

            return productListDtos;
        }
    }
}
