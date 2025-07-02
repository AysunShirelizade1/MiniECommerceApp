using MiniECommerceApp.Domain.Entities;
using MiniECommerceApp.Persistence.Contexts;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;
using MiniECommerce.Application.Services.Repositories;

namespace MiniECommerceApp.Persistence.Repositories
{
    public class CategoryRepository : Repository<Category>, ICategoryRepository
    {
        private readonly MiniECommerceDbContext _context;

        public CategoryRepository(MiniECommerceDbContext context) : base(context)
        {
            _context = context;
        }

        public Task AddAsync(object category)
        {
            throw new NotImplementedException();
        }

        public async Task<List<Category>> GetAllAsync()
        {
            return await _context.Categories.ToListAsync();
        }
    }
}
