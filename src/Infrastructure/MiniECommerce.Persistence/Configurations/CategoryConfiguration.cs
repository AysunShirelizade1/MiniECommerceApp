using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MiniECommerce.Domain.Entities;
namespace MiniECommerce.Persistence.Configurations;
public class CategoryConfiguration : IEntityTypeConfiguration<Category>
{
    public void Configure(EntityTypeBuilder<Category> builder)
    {
        builder.HasKey(c => c.Id);

        builder.Property(c => c.Name)
            .IsRequired()
            .HasMaxLength(100);

        builder.HasMany(c => c.SubCategories)
            .WithOne(sc => sc.ParentCategory)
            .HasForeignKey(sc => sc.ParentCategoryId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
