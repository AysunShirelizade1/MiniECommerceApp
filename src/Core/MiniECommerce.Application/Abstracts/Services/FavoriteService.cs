//using Microsoft.EntityFrameworkCore;
//using MiniECommerce.Application.Services.Interfaces;
//using MiniECommerce.Domain.Entities;
//using MiniECommerce.Domain.DTOs;
//using MiniECommerce.Persistence.Contexts;
//using MiniECommerce.Application.DTOs.ProductDto;
//using MiniECommerceApp.Domain.Entities;
//using MiniECommerceApp.Persistence.Contexts;

//namespace MiniECommerce.Application.Services
//{
//    public class FavoriteService : IFavoriteService
//    {
//        private readonly MiniECommerceDbContext _context;

//        public FavoriteService(MiniECommerceDbContext context)
//        {
//            _context = context;
//        }

//        public async Task<ServiceResult<bool>> AddToFavoritesAsync(Guid userId, Guid productId)
//        {
//            var existingFavorite = await _context.Favorites
//                .FirstOrDefaultAsync(f => f.UserId == userId && f.ProductId == productId);

//            if (existingFavorite != null)
//                return ServiceResult<bool>.Fail("Product already in favorites.");

//            var favorite = new Favorite
//            {
//                Id = Guid.NewGuid(),
//                UserId = userId,
//                ProductId = productId,
//                CreatedAt = DateTime.UtcNow,
//                CreatedUser = userId
//            };

//            _context.Favorites.Add(favorite);
//            await _context.SaveChangesAsync();

//            return ServiceResult<bool>.Success(true);
//        }

//        public async Task<ServiceResult<bool>> RemoveFromFavoritesAsync(Guid userId, Guid productId)
//        {
//            var favorite = await _context.Favorites
//                .FirstOrDefaultAsync(f => f.UserId == userId && f.ProductId == productId);

//            if (favorite == null)
//                return ServiceResult<bool>.Fail("Product not found in favorites.");

//            _context.Favorites.Remove(favorite);
//            await _context.SaveChangesAsync();

//            return ServiceResult<bool>.Success(true);
//        }

//        public async Task<IEnumerable<ProductDto>> GetFavoritesByUserIdAsync(Guid userId)
//        {
//            var favorites = await _context.Favorites
//                .Include(f => f.Product)
//                .ThenInclude(p => p.Images)
//                .Where(f => f.UserId == userId)
//                .ToListAsync();

//            return favorites.Select(f => new ProductDto(f.Product));
//        }
//    }
//}
