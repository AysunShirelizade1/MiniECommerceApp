using MiniECommerce.Application.DTOs.Product;
using MiniECommerceApp.Application.DTOs.Product;

namespace MiniECommerceApp.Application.Abstract
{
    public interface IProductService
    {
        Task<List<ProductListDto>> GetAllAsync();

        Task<ProductDetailDto?> GetByIdAsync(Guid id);

        Task CreateAsync(ProductCreateDto dto, Guid ownerId);

        Task<bool> UpdateAsync(Guid id, ProductUpdateDto dto, Guid ownerId);

        Task<bool> DeleteAsync(Guid id, Guid ownerId);
    }
}
