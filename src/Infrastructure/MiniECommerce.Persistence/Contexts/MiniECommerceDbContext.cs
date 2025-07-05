using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using MiniECommerce.Domain.Entities;
using MiniECommerce.Persistence.Contexts;

namespace MiniECommerce.Persistence.Contexts;

public class MiniECommerceDbContext : IdentityDbContext<AppUser, AppRole, Guid>
{
    public MiniECommerceDbContext(DbContextOptions<MiniECommerceDbContext> options)
        : base(options)
    {
    }

    public DbSet<Product> Products { get; set; } = null!;
    public DbSet<Category> Categories { get; set; } = null!;
    public DbSet<Order> Orders { get; set; } = null!;
    public DbSet<OrderProduct> OrderProducts { get; set; } = null!;
    public DbSet<Review> Reviews { get; set; } = null!;
    public DbSet<Favorite> Favorites { get; set; } = null!;
    public DbSet<Image> Images { get; set; } = null!;

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.ApplyConfigurationsFromAssembly(typeof(MiniECommerceDbContext).Assembly);
    }
}
