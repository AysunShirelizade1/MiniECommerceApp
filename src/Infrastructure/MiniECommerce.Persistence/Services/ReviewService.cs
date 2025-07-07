using MiniECommerce.Application.Abstracts.Services;
using MiniECommerce.Application.DTOs.ReviewDto;
using MiniECommerce.Domain.Entities;
using MiniECommerce.Persistence.Contexts;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MiniECommerce.Persistence.Services
{
    public class ReviewService : IReviewService
    {
        private readonly MiniECommerceDbContext _context;

        public ReviewService(MiniECommerceDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<ReviewDto>> GetAllAsync()
        {
            var reviews = await _context.Reviews.Include(r => r.User).ToListAsync();
            return reviews.Select(r => new ReviewDto(r));
        }

        public async Task<ReviewDto?> GetByIdAsync(Guid id)
        {
            var review = await _context.Reviews.FindAsync(id);
            if (review == null)
                return null;

            return new ReviewDto(review);
        }

        public async Task<Guid> CreateAsync(ReviewCreateDto dto, Guid userId)
        {
            var review = new Review
            {
                Id = Guid.NewGuid(),
                Comment = dto.Comment,
                UserId = userId,
                ProductId = dto.ProductId,
                // bunu real dəyər ilə əvəz et, yaxud dto-ya əlavə et
            };

            _context.Reviews.Add(review);
            await _context.SaveChangesAsync();
            return review.Id;
        }

        public async Task DeleteAsync(Guid id, Guid userId)
        {
            var review = await _context.Reviews.FindAsync(id);
            if (review == null)
                throw new KeyNotFoundException("Review tapılmadı.");

            // İstifadəçi öz şərhini silə bilər (və ya admin ola bilər)
            // Admin yoxlamasını burada əlavə etmək olar, əgər lazım olsa
            if (review.UserId != userId)
                throw new UnauthorizedAccessException("Bu şərhi silmək üçün icazəniz yoxdur.");

            _context.Reviews.Remove(review);
            await _context.SaveChangesAsync();
        }
    }
}
