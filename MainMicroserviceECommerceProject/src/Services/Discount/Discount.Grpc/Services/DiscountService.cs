

namespace Discount.Grpc.Services;

public class DiscountService(ILogger<DiscountService> logger, DiscountContext discountContext): DiscountProtoService.DiscountProtoServiceBase
{
    public override async Task<CouponModel> GetDiscount(GetDiscountRequest request, ServerCallContext context)
    {
        logger.LogInformation("GetDiscount called with ProductName: {ProductName}", request.ProductName);
        // TODO: Get the discount from the database or cache
        var coupon = await discountContext.Coupons.FirstOrDefaultAsync(c => c.ProductName == request.ProductName, context.CancellationToken);

        coupon ??= new Coupon
                {
                    ProductName = request.ProductName,
                    Amount = 0,
                    Description = "No discount found"
                };

        return coupon.Adapt<CouponModel>();
    }

    public override async Task<CouponModel> CreateDiscount(CreateDiscountRequest request, ServerCallContext context)
    {
        // TODO: Create a new discount in the database or cache
        logger.LogInformation("CreateDiscount called with ProductName: {ProductName}", request.Coupon.ProductName);
        var coupon = request.Coupon.Adapt<Coupon>() ?? throw new RpcException(new Status(StatusCode.InvalidArgument, "Invalid Request Object"));
        discountContext.Coupons.Add(coupon);
        await discountContext.SaveChangesAsync(context.CancellationToken);

        return request.Coupon;
    }
    
    public override async Task<CouponModel> UpdateDiscount(UpdateDiscountRequest request, ServerCallContext context)
    {
        // TODO: Update the existing discount in the database or cache
        logger.LogInformation("Executing the Update Discount Coupon for the product with Id {CouponId}", request.Coupon.Id);

        var coupon = await discountContext.Coupons.FirstOrDefaultAsync(c => c.Id == request.Coupon.Id, context.CancellationToken) ??
            throw new RpcException(new Status(StatusCode.NotFound, "Discount not found"));

        coupon.ProductName = request.Coupon.ProductName;
        coupon.Description = request.Coupon.Description;
        coupon.Amount = request.Coupon.Amount;          

        discountContext.Coupons.Update(coupon);
        await discountContext.SaveChangesAsync(context.CancellationToken);


        return request.Coupon;
    }
    public override async Task<DeleteDiscountResponse> DeleteDiscount(DeleteDiscountRequest request, ServerCallContext context)
    {
        // TODO: Delete the discount from the database or cache
        logger.LogInformation("Executing the Deleting Discount Coupon for the product with Id {CouponId}", request.ProductName);

        var coupon = await discountContext.Coupons.FirstOrDefaultAsync(c => c.ProductName == request.ProductName, context.CancellationToken) ??
            throw new RpcException(new Status(StatusCode.NotFound, "Discount not found"));

        discountContext.Coupons.Remove(coupon);
        await discountContext.SaveChangesAsync(context.CancellationToken);

        var response = new DeleteDiscountResponse
        {
            Success = true
        };
        return response;
    }

}
