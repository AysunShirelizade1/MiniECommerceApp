using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using MiniECommerce.Domain.Entities;
namespace MiniECommerce.Persistence.Configurations;
public class ImageConfiguration : IEntityTypeConfiguration<Image>
{
    public void Configure(EntityTypeBuilder<Image> builder)
    {
        builder.ToTable("ProductImages");

        builder.Property(i => i.ImageUrl)
            .IsRequired();

        builder.Property(i => i.IsMain)
            .HasDefaultValue(false);

        builder.HasOne(i => i.Product)
            .WithMany(p => p.Images)
            .HasForeignKey(i => i.ProductId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
