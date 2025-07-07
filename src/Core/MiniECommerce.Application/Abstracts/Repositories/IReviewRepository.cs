using MiniECommerce.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MiniECommerce.Persistence.Repositories
{
    public interface IReviewRepository
    {
        Task<IEnumerable<Review>> GetAllAsync();
        Task<Review?> GetByIdAsync(Guid id);
        Task AddAsync(Review review);
        void Remove(Review review);
        Task SaveChangesAsync();
    }
}
