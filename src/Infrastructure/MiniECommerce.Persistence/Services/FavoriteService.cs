using MiniECommerce.Application.Abstractions.Services;
using MiniECommerce.Application.DTOs.Favorite;
using MiniECommerce.Application.Abstracts.Repositories;
using MiniECommerce.Domain.Entities;


namespace MiniECommerce.Persistence.Services;

public class FavoriteService : IFavoriteService
{
    private readonly IFavoriteRepository _favoriteRepository;

    public FavoriteService(IFavoriteRepository favoriteRepository)
    {
        _favoriteRepository = favoriteRepository;
    }

    public async Task<List<FavoriteDto>> GetAllByUserIdAsync(Guid userId)
    {
        var favorites = await _favoriteRepository.GetAllByUserIdAsync(userId);
        return favorites.Select(f => new FavoriteDto
        {
            Id = f.Id,
            ProductId = f.ProductId,
            ProductTitle = f.Product.Title, 
            ProductImage = f.Product.Images.FirstOrDefault(i => i.IsMain)?.ImageUrl ?? string.Empty,
            Price = f.Product.Price
        }).ToList();
    }

    public async Task<FavoriteDto?> GetByIdAsync(Guid id)
    {
        var favorite = await _favoriteRepository.GetByIdAsync(id);
        if (favorite == null) return null;

        return new FavoriteDto
        {
            Id = favorite.Id,
            ProductId = favorite.ProductId,
            ProductTitle = favorite.Product.Title,
            ProductImage = favorite.Product.Images.FirstOrDefault(i => i.IsMain)?.ImageUrl ?? string.Empty,
            Price = favorite.Product.Price
        };
    }

    public async Task<Guid> CreateAsync(Guid userId, CreateFavoriteDto dto)
    {
        var favorite = new Favorite
        {
            Id = Guid.NewGuid(),
            AppUserId = userId,
            ProductId = dto.ProductId
        };

        await _favoriteRepository.AddAsync(favorite);
        await _favoriteRepository.SaveChangeAsync();

        return favorite.Id;
    }

    public async Task DeleteAsync(Guid id, Guid userId)
    {
        var favorite = await _favoriteRepository.GetByIdAsync(id);
        if (favorite == null) throw new Exception("Favorite tapılmadı.");
        if (favorite.AppUserId != userId) throw new UnauthorizedAccessException("Bu əməliyyatı həyata keçirməyə icazəniz yoxdur.");

        _favoriteRepository.Delete(favorite);
        await _favoriteRepository.SaveChangeAsync();
    }
}
