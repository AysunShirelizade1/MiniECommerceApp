using AutoMapper;
using MiniECommerce.Application.DTOs.Product;
using MiniECommerceApp.Application.Abstract;
using MiniECommerceApp.Application.DTOs.Product;
using MiniECommerceApp.Domain.Entities;

namespace MiniECommerceApp.Persistence.Services
{
    public class ProductService : IProductService
    {
        private readonly IProductRepository _productRepository;
        private readonly IMapper _mapper;

        public ProductService(IProductRepository productRepository, IMapper mapper)
        {
            _productRepository = productRepository;
            _mapper = mapper;
        }

        public async Task<List<ProductListDto>> GetAllAsync()
        {
            var products = await _productRepository.GetProductsWithIncludesAsync();
            return _mapper.Map<List<ProductListDto>>(products);
        }

        public async Task<ProductDetailDto?> GetByIdAsync(Guid id)
        {
            var product = await _productRepository.GetProductDetailAsync(id);
            return product is not null ? _mapper.Map<ProductDetailDto>(product) : null;
        }

        // CreateAsync metoduna ownerId əlavə edildi
        public async Task CreateAsync(ProductCreateDto dto, Guid ownerId)
        {
            var product = _mapper.Map<Product>(dto);
            product.OwnerId = ownerId;
            await _productRepository.AddAsync(product);
            await _productRepository.SaveChangeAsync();
        }

        // UpdateAsync metodunda yoxlama edildi ki, yalnız sahibi redaktə edə bilər
        public async Task<bool> UpdateAsync(Guid id, ProductUpdateDto dto, Guid ownerId)
        {
            var product = await _productRepository.GetByIdAsync(id);
            if (product == null || product.OwnerId != ownerId)
                return false;

            _mapper.Map(dto, product);
            _productRepository.Update(product);
            await _productRepository.SaveChangeAsync();
            return true;
        }

        // DeleteAsync metodunda da eyni yoxlama
        public async Task<bool> DeleteAsync(Guid id, Guid ownerId)
        {
            var product = await _productRepository.GetByIdAsync(id);
            if (product == null || product.OwnerId != ownerId)
                return false;

            _productRepository.Delete(product);
            await _productRepository.SaveChangeAsync();
            return true;
        }
    }
}
