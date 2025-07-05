using MiniECommerce.Domain.Entities;

namespace MiniECommerce.Application.Abstracts.Repositories;

public interface IImageRepository : IRepository<Image>
{
    Task<List<Image>> GetAllByProductIdAsync(Guid productId);
}
