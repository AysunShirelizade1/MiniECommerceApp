using AutoMapper;
using MiniECommerce.Application.DTOs.Product;
using MiniECommerceApp.Application.DTOs.Product;

public class ProductMappingProfile : Profile
{
    public ProductMappingProfile()
    {
        CreateMap<Product, ProductListDto>().ReverseMap();
        CreateMap<Product, ProductDetailDto>().ReverseMap();
        CreateMap<Product, ProductCreateDto>().ReverseMap();
        CreateMap<Product, ProductUpdateDto>().ReverseMap();
    }
}
