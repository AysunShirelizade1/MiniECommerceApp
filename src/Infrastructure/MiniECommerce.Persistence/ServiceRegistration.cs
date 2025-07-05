using Microsoft.Extensions.DependencyInjection;
using MiniECommerce.Application.Abstracts.Services;
using MiniECommerceApp.Application.Repositories;
using MiniECommerceApp.Persistence.Repositories;

namespace MiniECommerce.Persistence;

public static class ServiceRegistration
{
    public static void RegisterService(this IServiceCollection services)
    {
        #region Repositories
        services.AddScoped(typeof(IRepository<>), typeof(Repository<>));

        #endregion
        #region Servicies

        services.AddScoped<IProductService, ProductService>();



        #endregion

    }
}
