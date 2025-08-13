

namespace Discount.Grpc.DataConfigurations;

public class CouponModelConfiguration : IEntityTypeConfiguration<Coupon>
{
    public void Configure(EntityTypeBuilder<Coupon> builder)
    {
        builder.HasData(
            new Coupon
            {
                Id = 1,
                ProductName = "IPhone X",
                Description = "IPhone Discount",
                Amount = 150
            },
            new Coupon
            {
                Id = 2,
                ProductName = "Samsung S10",
                Description = "Samsung Discount",
                Amount = 100
            },
            new Coupon
            {
                Id = 3,
                ProductName = "Google Pixel",
                Description = "Google Discount",
                Amount = 50
            }
        );
    }
}
