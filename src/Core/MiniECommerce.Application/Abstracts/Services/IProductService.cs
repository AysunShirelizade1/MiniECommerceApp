using MiniECommerce.Application.DTOs.Product;

namespace MiniECommerce.Application.Abstracts.Services;

public interface IProductService
{
    Task<IEnumerable<ProductListDto>> GetAllAsync();
    Task<ProductDetailDto?> GetByIdAsync(Guid id);
    Task CreateAsync(ProductCreateDto dto);
    Task UpdateAsync(Guid id, ProductUpdateDto dto);
    Task DeleteAsync(Guid id);
}
