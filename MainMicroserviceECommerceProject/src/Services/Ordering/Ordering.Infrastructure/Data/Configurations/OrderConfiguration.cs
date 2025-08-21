
namespace Ordering.Infrastructure.Data.Configurations;

public class OrderConfiguration : IEntityTypeConfiguration<Order>
{
    public void Configure(EntityTypeBuilder<Order> builder)
    {
        builder.HasKey(o => o.Id);
        builder.Property(o => o.Id)
            .HasConversion(id => id.Value, value => OrderId.Of(value))
            .IsRequired();

        builder.HasOne<Customer>()
            .WithMany()
            .HasForeignKey(o => o.Customer)
            .IsRequired();

        builder.HasMany(o => o.OrderItems).WithOne()
            .HasForeignKey(oi => oi.OrderId)
            .IsRequired();

        builder.ComplexProperty(o => o.OrderName, nameBuilder =>
        {
            nameBuilder.Property(n => n.Value).HasColumnName(nameof(Order.OrderName))
            .HasMaxLength(100).IsRequired();
        });

        builder.ComplexProperty(o => o.ShippingAddress, addressBuilder =>
        {
            addressBuilder.Property(a => a.FirstName).HasMaxLength(100).IsRequired();
            addressBuilder.Property(a => a.LastName).HasMaxLength(100).IsRequired();
            addressBuilder.Property(a => a.AddressLine).HasMaxLength(200).IsRequired();
            addressBuilder.Property(a => a.State).HasMaxLength(50).IsRequired();
            addressBuilder.Property(a => a.Country).HasMaxLength(50).IsRequired();
            addressBuilder.Property(a => a.ZipCode).HasMaxLength(5).IsRequired();

        });

        builder.ComplexProperty(o => o.BillingAddress, addressBuilder =>
        {
            addressBuilder.Property(a => a.FirstName).HasMaxLength(100).IsRequired();
            addressBuilder.Property(a => a.LastName).HasMaxLength(100).IsRequired();
            addressBuilder.Property(a => a.AddressLine).HasMaxLength(200).IsRequired();
            addressBuilder.Property(a => a.State).HasMaxLength(50).IsRequired();
            addressBuilder.Property(a => a.Country).HasMaxLength(50).IsRequired();
            addressBuilder.Property(a => a.ZipCode).HasMaxLength(5).IsRequired();

        });

        builder.ComplexProperty(o => o.Payment, paymentBuilder =>
        {
            paymentBuilder.Property(p => p.CardNumber).HasMaxLength(24).IsRequired();
            paymentBuilder.Property(p => p.CardName).HasMaxLength(100).IsRequired();
            paymentBuilder.Property(p => p.Expiration).IsRequired();
            paymentBuilder.Property(p => p.CVV).HasMaxLength(3).IsRequired();
            paymentBuilder.Property(p => p.PaymentMethod).IsRequired();
        });

        builder.Property(o => o.Status)
            .HasDefaultValue(OrderStatus.Pending)
            .HasConversion(s => s.ToString(), value => Enum.Parse<OrderStatus>(value))
            .IsRequired();

        builder.Property(o => o.TotalPrice);
    }
}
