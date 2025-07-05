using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MiniECommerce.Domain.Entities;

namespace MiniECommerce.Persistence.Configuration;

public class FavoriteConfiguration : IEntityTypeConfiguration<Favorite>
{
    public void Configure(EntityTypeBuilder<Favorite> builder)
    {
        builder.HasKey(f => f.Id);

        builder.HasIndex(f => new { f.AppUserId, f.ProductId })
            .IsUnique();

        builder.HasOne(f => f.AppUser)
            .WithMany()
            .HasForeignKey(f => f.AppUserId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne(f => f.Product)
            .WithMany(p => p.Favorites)
            .HasForeignKey(f => f.ProductId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
