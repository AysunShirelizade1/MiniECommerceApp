//using Microsoft.EntityFrameworkCore;
//using MiniECommerce.Application.Common;
//using MiniECommerce.Application.DTOs.CategoryDto;
//using MiniECommerce.Application.Services.Interfaces;
//using MiniECommerceApp.Domain.Entities;
//using MiniECommerceApp.Persistence.Contexts;

//namespace MiniECommerce.Application.Services
//{
//    public class CategoryService : ICategoryService
//    {
//        private readonly MiniECommerceDbContext _context;

//        public CategoryService(MiniECommerceDbContext context)
//        {
//            _context = context;
//        }

//        public async Task<IEnumerable<CategoryDto>> GetAllAsync()
//        {
//            var categories = await _context.Categories
//                .Include(c => c.SubCategories)
//                .Where(c => c.ParentCategoryId == null)
//                .ToListAsync();

//            return categories.Select(c => new CategoryDto(c));
//        }

//        public async Task<ServiceResult<CategoryDto>> CreateAsync(CategoryCreateDto dto)
//        {
//            var category = new Category
//            {
//                Id = Guid.NewGuid(),
//                Name = dto.Name,
//                ParentCategoryId = dto.ParentCategoryId
//            };

//            _context.Categories.Add(category);
//            await _context.SaveChangesAsync();

//            return ServiceResult<CategoryDto>.Success(new CategoryDto(category));
//        }
//    }
//}
