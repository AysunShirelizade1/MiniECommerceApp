using Microsoft.EntityFrameworkCore;
using MiniECommerce.Application.Abstracts.Repositories;
using MiniECommerce.Domain.Entities;
using MiniECommerce.Persistence.Contexts;

namespace MiniECommerce.Persistence.Repositories
{
    public class ImageRepository : Repository<Image>, IImageRepository
    {
        private readonly MiniECommerceDbContext _context;

        public ImageRepository(MiniECommerceDbContext context) : base(context)
        {
            _context = context;
        }

        public async Task<List<Image>> GetAllByProductIdAsync(Guid productId)
        {
            return await _context.Images
                .Where(i => i.ProductId == productId)
                .ToListAsync();
        }
    }
}
