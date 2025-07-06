using Microsoft.Extensions.DependencyInjection;
using MiniECommerce.Application.Abstracts.Repositories;
using MiniECommerce.Application.Abstracts.Services;
using MiniECommerce.Persistence.Repositories;
using MiniECommerce.Persistence.Services;
namespace MiniECommerce.Persistence;

public static class ServiceRegistration
{
    public static void RegisterService(this IServiceCollection services)
    {
        #region Repositories
        services.AddScoped(typeof(IRepository<>), typeof(Repository<>));
        services.AddScoped<ICategoryRepository, CategoryRepository>();
        services.AddScoped<IProductRepository, ProductRepository>();
        services.AddScoped<IOrderRepository, OrderRepository>();
        services.AddScoped<IFavoriteRepository, FavoriteRepository>();
        services.AddScoped<IImageRepository, ImageRepository>();


        #endregion

        #region Servicies

        services.AddScoped<IProductService, ProductService>();
        services.AddScoped<ICategoryService, CategoryService>();
        services.AddScoped<IOrderService, OrderService>();
        services.AddScoped<IFavoriteService, FavoriteService>();
        services.AddScoped<IUserService, UserService>();
        services.AddScoped<IImageService, ImageService>();
        #endregion


    }
}
