using Microsoft.EntityFrameworkCore;
using MiniECommerce.Domain.Entities;

namespace MiniECommerce.Persistence.Contexts;

public class MiniECommerceDbContext : DbContext
{
    public MiniECommerceDbContext(DbContextOptions<MiniECommerceDbContext> options) : base(options)
    {
    }

    public DbSet<Product> Products { get; set; }
    public DbSet<Category> Categories { get; set; }
    public DbSet<Order> Orders { get; set; }
    public DbSet<Review> Reviews { get; set; }
    public DbSet<Favorite> Favorites { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.ApplyConfigurationsFromAssembly(typeof(MiniECommerceDbContext).Assembly);

        
    }
}
