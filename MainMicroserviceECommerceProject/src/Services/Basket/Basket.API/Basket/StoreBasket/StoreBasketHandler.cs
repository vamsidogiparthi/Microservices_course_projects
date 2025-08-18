using Discount.Grpc.Protos;

namespace Basket.API.Basket.StoreBasket;

public record StoreBasketCommand(ShoppingCart ShoppingCart) : ICommand<StoreBasketCommandResult>;
public record StoreBasketCommandResult(string UserName);

public class StoreBasketCommandValidator: AbstractValidator<StoreBasketCommand>
{
    public StoreBasketCommandValidator()
    {
        RuleFor(x => x.ShoppingCart).NotNull().WithMessage("Shopping cart cannot be null.");
        RuleFor(x => x.ShoppingCart.Items).NotEmpty().WithMessage("Shopping cart must contain at least one item.");
        RuleFor(x => x.ShoppingCart.UserName).NotEmpty().WithMessage("User name cannot be empty.");            
    }
}

public class StoreBasketCommandHandler(IBasketRepository db, DiscountProtoService.DiscountProtoServiceClient discountClient) : ICommandHandler<StoreBasketCommand, StoreBasketCommandResult>
{
    public async Task<StoreBasketCommandResult> Handle(StoreBasketCommand command, CancellationToken cancellationToken)
    {

        command.ShoppingCart.Items = await DeductDiscount(command, cancellationToken);
        // store the basket in the database and update the cache.
        await db.StoreBasketAsync(command.ShoppingCart, cancellationToken);
        // Return a successful result
        return new StoreBasketCommandResult(command.ShoppingCart.UserName);
    }

    public async Task<List<ShoppingCartItem>> DeductDiscount(StoreBasketCommand command, CancellationToken cancellationToken)
    {
        foreach (var item in command.ShoppingCart.Items)
        {
            // Check if the item has a discount
            var discount = await discountClient.GetDiscountAsync(new GetDiscountRequest { ProductName = item.ProductName }, cancellationToken: cancellationToken);
            if (discount != null && discount.Amount > 0)
            {
                // Apply the discount to the item price
                item.Price -= (decimal)discount.Amount;
            }
        }
        return command.ShoppingCart.Items;
    }
}
