using Microsoft.EntityFrameworkCore;
using MiniECommerce.Application.Abstracts.Repositories;
using MiniECommerce.Domain.Entities;
using MiniECommerce.Persistence.Contexts;

namespace MiniECommerce.Persistence.Repositories;

public class FavoriteRepository : Repository<Favorite>, IFavoriteRepository
{
    private readonly MiniECommerceDbContext _context;

    public FavoriteRepository(MiniECommerceDbContext context) : base(context)
    {
        _context = context;
    }

    public async Task<List<Favorite>> GetAllByUserIdAsync(Guid userId)
    {
        return await _context.Favorites
            .Include(f => f.Product)
                .ThenInclude(p => p.Images)
            .Where(f => f.AppUserId == userId)
            .ToListAsync();
    }
}
