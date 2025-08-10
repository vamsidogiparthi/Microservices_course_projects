using Discount.Grpc.Protos;
using Grpc.Core;

namespace Discount.Grpc.Services;

public class DiscountServiceClass: DiscountProtoService.DiscountProtoServiceBase
{
    public override Task<CouponModel> GetDiscount(GetDiscountRequest request, ServerCallContext context)
    {
        // TODO: Get the discount from the database or cache
        return base.GetDiscount(request, context);
    }

    public override Task<CouponModel> CreateDiscount(CreateDiscountRequest request, ServerCallContext context)
    {
        // TODO: Create a new discount in the database or cache
        return base.CreateDiscount(request, context);
    }
    
    public override Task<CouponModel> UpdateDiscount(UpdateDiscountRequest request, ServerCallContext context)
    {
        // TODO: Update the existing discount in the database or cache
        return base.UpdateDiscount(request, context);
    }
    public override Task<DeleteDiscountResponse> DeleteDiscount(DeleteDiscountRequest request, ServerCallContext context)
    {
        // TODO: Delete the discount from the database or cache
        return base.DeleteDiscount(request, context);
    }

}
