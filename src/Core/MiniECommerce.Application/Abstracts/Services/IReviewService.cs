//using Microsoft.EntityFrameworkCore;
//using MiniECommerce.Application.Common;
//using MiniECommerce.Application.DTOs.ReviewDto;
//using MiniECommerce.Application.Services.Interfaces;
//using MiniECommerceApp.Domain.Entities;
//using MiniECommerceApp.Persistence.Contexts;

//namespace MiniECommerce.Application.Services
//{
//    public class ReviewService : IReviewService
//    {
//        private readonly MiniECommerceDbContext _context;

//        public ReviewService(MiniECommerceDbContext context)
//        {
//            _context = context;
//        }

//        public async Task<ServiceResult<ReviewDto>> AddReviewAsync(Guid productId, ReviewCreateDto dto, Guid userId)
//        {
//            var product = await _context.Products.FindAsync(productId);
//            if (product == null)
//                return ServiceResult<ReviewDto>.Fail("Product not found.");

//            var review = new Review
//            {
//                Id = Guid.NewGuid(),
//                ProductId = productId,
//                Comment = dto.Comment,
//                CreatedAt = DateTime.UtcNow,
//                CreatedUser = userId
//            };

//            _context.Reviews.Add(review);
//            await _context.SaveChangesAsync();

//            return ServiceResult<ReviewDto>.Success(new ReviewDto(review));
//        }

//        public async Task<IEnumerable<ReviewDto>> GetReviewsByProductIdAsync(Guid productId)
//        {
//            var reviews = await _context.Reviews
//                .Where(r => r.ProductId == productId)
//                .ToListAsync();

//            return reviews.Select(r => new ReviewDto(r));
//        }
//    }
//}
