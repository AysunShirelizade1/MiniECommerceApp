using MiniECommerce.Domain.Entities;
using MiniECommerce.Persistence.Contexts;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MiniECommerce.Persistence.Repositories
{
    public class ReviewRepository : IReviewRepository
    {
        private readonly MiniECommerceDbContext _context;

        public ReviewRepository(MiniECommerceDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Review>> GetAllAsync()
        {
            return await _context.Reviews
                .Include(r => r.User)
                .ToListAsync();
        }

        public async Task<Review?> GetByIdAsync(Guid id)
        {
            return await _context.Reviews.FindAsync(id);
        }

        public async Task AddAsync(Review review)
        {
            await _context.Reviews.AddAsync(review);
        }

        public void Remove(Review review)
        {
            _context.Reviews.Remove(review);
        }

        public async Task SaveChangesAsync()
        {
            await _context.SaveChangesAsync();
        }
    }
}
