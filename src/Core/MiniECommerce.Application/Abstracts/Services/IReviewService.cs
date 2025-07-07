using MiniECommerce.Application.DTOs.ReviewDto;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MiniECommerce.Application.Abstracts.Services
{
    public interface IReviewService
    {
        Task<IEnumerable<ReviewDto>> GetAllAsync();
        Task<ReviewDto?> GetByIdAsync(Guid id);
        Task<Guid> CreateAsync(ReviewCreateDto dto, Guid userId);
        Task DeleteAsync(Guid id, Guid userId);
    }
}
