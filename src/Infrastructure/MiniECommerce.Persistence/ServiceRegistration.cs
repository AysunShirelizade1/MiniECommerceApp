using Microsoft.Extensions.DependencyInjection;
using MiniECommerceApp.Application.Abstract;
using MiniECommerceApp.Persistence.Repositories;
using MiniECommerceApp.Persistence.Services;
using MiniECommerce.Application.Services;

namespace MiniECommerce.Persistence;

public static class ServiceRegistration
{
    public static void RegisterService(this IServiceCollection services)
    {
        #region Repositories
        services.AddScoped<IProductRepository, ProductRepository>();
        services.AddScoped<ICategoryRepository, CategoryRepository>();

        #endregion
        #region Servicies

        services.AddScoped<ICategoryService, CategoryService>();
        services.AddScoped<IProductService, ProductService>();
        services.AddScoped<IAuthService, AuthService>();


        #endregion

    }
}
