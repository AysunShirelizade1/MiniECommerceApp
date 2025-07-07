using Microsoft.Extensions.DependencyInjection;
using MiniECommerce.Application.Abstracts.Repositories;
using MiniECommerce.Application.Abstracts.Services;
using MiniECommerce.Persistence.Repositories;
using MiniECommerce.Persistence.Services;
using MiniECommerce.Infrastructure.Services;
using Microsoft.Extensions.Configuration;

namespace MiniECommerce.Persistence;

public static class ServiceRegistration
{
    public static void RegisterService(this IServiceCollection services, IConfiguration configuration)
    {
        #region Repositories
        services.AddScoped(typeof(IRepository<>), typeof(Repository<>));
        services.AddScoped<ICategoryRepository, CategoryRepository>();
        services.AddScoped<IProductRepository, ProductRepository>();
        services.AddScoped<IOrderRepository, OrderRepository>();
        services.AddScoped<IFavoriteRepository, FavoriteRepository>();
        services.AddScoped<IImageRepository, ImageRepository>();
        services.AddScoped<IReviewRepository, ReviewRepository>();
        #endregion

        #region Services
        services.Configure<SmtpSettings>(configuration.GetSection("SmtpSettings"));
        services.AddTransient<IEmailService, EmailService>();


        services.AddScoped<IProductService, ProductService>();
        services.AddScoped<ICategoryService, CategoryService>();
        services.AddScoped<IOrderService, OrderService>();
        services.AddScoped<IFavoriteService, FavoriteService>();
        services.AddScoped<IUserService, UserService>();
        services.AddScoped<IImageService, ImageService>();
        services.AddScoped<IReviewService, ReviewService>();
        #endregion
    }

}
