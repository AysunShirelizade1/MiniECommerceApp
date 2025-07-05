using MiniECommerce.Domain.Entities;

namespace MiniECommerce.Application.Abstracts.Repositories;

public interface IFavoriteRepository : IRepository<Favorite>
{
    Task<List<Favorite>> GetAllByUserIdAsync(Guid userId);
}
