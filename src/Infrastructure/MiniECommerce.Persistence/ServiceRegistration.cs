using Microsoft.Extensions.DependencyInjection;
using MiniECommerceApp.Application.Abstract;
using MiniECommerceApp.Persistence.Repositories;
using MiniECommerceApp.Persistence.Services;
namespace MiniECommerce.Persistence;

public static class ServiceRegistration
{
    public static void RegisterService(this IServiceCollection services)
    {
        #region Repositories
        services.AddScoped<IProductRepository, ProductRepository>();


        #endregion
        #region Servicies

        services.AddScoped<IProductService, ProductService>();


        #endregion

    }
}
