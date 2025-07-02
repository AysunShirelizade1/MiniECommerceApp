using MiniECommerce.Application.DTOs.Product;
using MiniECommerceApp.Application.DTOs.Product;

namespace MiniECommerce.Application.Services
{
    public interface IProductService
    {
        Task<List<ProductListDto>> GetAllFilteredAsync(Guid? categoryId, decimal? minPrice, decimal? maxPrice, string? search);
        Task<ProductDetailDto?> GetByIdAsync(Guid id);
        Task<Guid> CreateAsync(ProductCreateDto dto, Guid ownerId); // ← düzəliş buradadır
        Task<bool> UpdateAsync(Guid id, ProductUpdateDto dto, Guid ownerId);
        Task<bool> DeleteAsync(Guid id, Guid ownerId);
        Task<List<ProductListDto>> GetProductsByOwnerAsync(Guid ownerId);
    }
}
