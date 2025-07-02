//using Microsoft.EntityFrameworkCore;
//using MiniECommerce.Application.Common;
//using MiniECommerce.Application.DTOs.ProductDto;
//using MiniECommerce.Application.Services.Interfaces;
//using MiniECommerceApp.Persistence.Contexts;

//namespace MiniECommerce.Application.Services
//{
//    public class ProductService : IProductService
//    {
//        private readonly MiniECommerceDbContext _context;

//        public ProductService(MiniECommerceDbContext context)
//        {
//            _context = context;
//        }

//        public async Task<IEnumerable<ProductDto>> GetAllAsync(ProductFilterDto filter)
//        {
//            var query = _context.Products
//                .Include(p => p.Category)
//                .Include(p => p.Images)
//                .AsQueryable();

//            if (filter.CategoryId != null)
//                query = query.Where(p => p.CategoryId == filter.CategoryId);

//            if (filter.MinPrice != null)
//                query = query.Where(p => p.Price >= filter.MinPrice);

//            if (filter.MaxPrice != null)
//                query = query.Where(p => p.Price <= filter.MaxPrice);

//            if (!string.IsNullOrWhiteSpace(filter.Search))
//                query = query.Where(p => p.Title.Contains(filter.Search) || p.Description.Contains(filter.Search));

//            var products = await query.ToListAsync();

//            return products.Select(p => new ProductDto(p));
//        }

//        public async Task<ProductDto?> GetByIdAsync(Guid id)
//        {
//            var product = await _context.Products
//                .Include(p => p.Category)
//                .Include(p => p.Images)
//                .Include(p => p.Reviews)
//                .Include(p => p.Favorites)
//                .FirstOrDefaultAsync(p => p.Id == id);

//            if (product == null)
//                return null;

//            return new ProductDto(product);
//        }

//        public async Task<ServiceResult<ProductDto>> CreateAsync(ProductCreateDto dto, Guid ownerId)
//        {
//            var product = new Domain.Entities.Product
//            {
//                Id = Guid.NewGuid(),
//                Title = dto.Title,
//                Description = dto.Description,
//                Price = dto.Price,
//                OwnerId = ownerId,
//                CategoryId = dto.CategoryId,
//                Images = new List<MiniECommerceApp.Domain.Entities.Image>()
//            };

//            if (dto.ImageUrls != null)
//            {
//                foreach (var url in dto.ImageUrls)
//                {
//                    product.Images.Add(new MiniECommerceApp.Domain.Entities.Image
//                    {
//                        Id = Guid.NewGuid(),
//                        ImageUrl = url,
//                        IsMain = false,
//                        ProductId = product.Id
//                    });
//                }
//            }

//            _context.Products.Add(product);
//            await _context.SaveChangesAsync();

//            return ServiceResult<ProductDto>.Success(new ProductDto(product));
//        }

//        public async Task<ServiceResult<ProductDto>> UpdateAsync(Guid id, ProductUpdateDto dto, Guid ownerId)
//        {
//            var product = await _context.Products.Include(p => p.Images).FirstOrDefaultAsync(p => p.Id == id);

//            if (product == null)
//                return ServiceResult<ProductDto>.Fail("Product not found.");

//            if (product.OwnerId != ownerId)
//                return ServiceResult<ProductDto>.Fail("Unauthorized.");

//            product.Title = dto.Title;
//            product.Description = dto.Description;
//            product.Price = dto.Price;
//            product.CategoryId = dto.CategoryId;

//            // Images update logic can be added here

//            await _context.SaveChangesAsync();

//            return ServiceResult<ProductDto>.Success(new ProductDto(product));
//        }

//        public async Task<ServiceResult<bool>> DeleteAsync(Guid id, Guid ownerId)
//        {
//            var product = await _context.Products.FirstOrDefaultAsync(p => p.Id == id);

//            if (product == null)
//                return ServiceResult<bool>.Fail("Product not found.");

//            if (product.OwnerId != ownerId)
//                return ServiceResult<bool>.Fail("Unauthorized.");

//            _context.Products.Remove(product);
//            await _context.SaveChangesAsync();

//            return ServiceResult<bool>.Success(true);
//        }

//        public async Task<IEnumerable<ProductDto>> GetByOwnerIdAsync(Guid ownerId)
//        {
//            var products = await _context.Products
//                .Where(p => p.OwnerId == ownerId)
//                .Include(p => p.Images)
//                .ToListAsync();

//            return products.Select(p => new ProductDto(p));
//        }

//    }
//}
