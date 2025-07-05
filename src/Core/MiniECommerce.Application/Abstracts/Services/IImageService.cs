using MiniECommerce.Application.DTOs.Image;

namespace MiniECommerce.Application.Abstracts.Services;

public interface IImageService
{
    Task<List<ImageDto>> GetAllByProductIdAsync(Guid productId);
    Task<ImageDto?> GetByIdAsync(Guid id);
    Task CreateAsync(CreateImageDto dto);
    Task UpdateAsync(Guid id, UpdateImageDto dto);
    Task DeleteAsync(Guid id);
}
