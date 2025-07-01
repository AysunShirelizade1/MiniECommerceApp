using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MiniECommerce.Domain.Entities;
namespace MiniECommerce.Persistence.Configurations;
public class FavoriteConfiguration : IEntityTypeConfiguration<Favorite>
{
    public void Configure(EntityTypeBuilder<Favorite> builder)
    {
        builder.HasKey(f => f.Id);

        // UserId xarici açar yoxdur, sadəcə property kimi saxla

        // Məsələn:
        builder.Property(f => f.Id)
            .IsRequired();

        builder.HasOne<Product>()
            .WithMany()
            .HasForeignKey(f => f.ProductId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
