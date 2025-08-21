
namespace Ordering.Infrastructure.Data.Configurations;

public class ProductConfiguration : IEntityTypeConfiguration<Product>
{
    public void Configure(EntityTypeBuilder<Product> builder)
    {
        builder.HasKey(p => p.Id);
        builder.Property(p => p.Id).HasConversion(p => p.Value, dbId => ProductId.Of(dbId));

        builder.Property(p => p.Name)
            .IsRequired()
            .HasMaxLength(100);
        
    }
}
