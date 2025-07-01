using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using MiniECommerce.Domain.Entities;
namespace MiniECommerce.Persistence.Configurations;
public class OrderProductConfiguration : IEntityTypeConfiguration<OrderProduct>
{
    public void Configure(EntityTypeBuilder<OrderProduct> builder)
    {
        builder.ToTable("OrderProducts");

        builder.HasOne(op => op.Order)
            .WithMany(o => o.OrderProducts)
            .HasForeignKey(op => op.Id);

        builder.HasOne(op => op.Product)
            .WithMany(p => p.OrderProducts)
            .HasForeignKey(op => op.Id);

        builder.Property(op => op.Quantity)
            .IsRequired();

        builder.Property(op => op.UnitPrice)
            .HasPrecision(10, 2);
    }
}
