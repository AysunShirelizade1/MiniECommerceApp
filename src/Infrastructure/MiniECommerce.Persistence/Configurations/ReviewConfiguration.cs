using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MiniECommerce.Domain.Entities;
namespace MiniECommerce.Persistence.Configurations;
public class ReviewConfiguration : IEntityTypeConfiguration<Review>
{
    public void Configure(EntityTypeBuilder<Review> builder)
    {
        builder.HasKey(r => r.Id);

        builder.Property(r => r.Comment)
            .HasMaxLength(1000);

        builder.Property(r => r.Rating)
            .IsRequired();

        builder.Property(r => r.CreatedAt)
            .HasDefaultValueSql("GETDATE()");

        // UserId xarici açar yox, sadəcə property kimi saxla
    }
}
