using MiniECommerce.Application.DTOs.Favorite;

namespace MiniECommerce.Application.Abstracts.Services;

public interface IFavoriteService
{
    Task<List<FavoriteDto>> GetAllByUserIdAsync(Guid userId);
    Task<FavoriteDto?> GetByIdAsync(Guid id);
    Task<Guid> CreateAsync(Guid userId, CreateFavoriteDto dto);
    Task DeleteAsync(Guid id, Guid userId);
}
