using MiniECommerceApp.Application.Repositories;
using MiniECommerceApp.Domain.Entities;

namespace MiniECommerceApp.Persistence.Repositories;

public interface ICategoryRepository : IRepository<Category>
{
    Task AddAsync(object category);
    Task<List<Category>> GetAllAsync();
}