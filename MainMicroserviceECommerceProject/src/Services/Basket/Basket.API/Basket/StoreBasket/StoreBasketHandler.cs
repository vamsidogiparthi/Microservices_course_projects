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

public class StoreBasketCommandHandler(IBasketRepository db) : ICommandHandler<StoreBasketCommand, StoreBasketCommandResult>
{
    public async Task<StoreBasketCommandResult> Handle(StoreBasketCommand command, CancellationToken cancellationToken)
    {
        // store the basket in the database and update the cache.
        await db.StoreBasketAsync(command.ShoppingCart, cancellationToken);
        // Return a successful result
        return new StoreBasketCommandResult(command.ShoppingCart.UserName);
    }
}
