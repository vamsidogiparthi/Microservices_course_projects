
namespace Ordering.Infrastructure.Data.Configurations;

public class OrderItemConfiguration : IEntityTypeConfiguration<OrderItem>
{
    public void Configure(EntityTypeBuilder<OrderItem> builder)
    {
        builder.HasKey(o => o.Id);
        builder.Property(o => o.Id)
            .HasConversion(o => o.Value,
                           dbId => OrderItemId.Of(dbId));
        builder.HasOne<Product>().WithMany().HasForeignKey(o => o.ProductId);
        builder.Property(o => o.Quantity).IsRequired();
        builder.Property(o => o.Price)
            .HasPrecision(18, 2)
            .IsRequired();
    }
}
