namespace Basket.API.Basket.CheckoutBasket;

public record CheckoutBasketCommand(BasketCheckoutDto BasketCheckout) : ICommand<CheckoutBasketResult>;
public record CheckoutBasketResult(bool IsSuccess);

public class CheckoutBasketCommandValidator: AbstractValidator<CheckoutBasketCommand>
{
    public CheckoutBasketCommandValidator()
    {
        RuleFor(x => x.BasketCheckout).NotNull();
        RuleFor(x => x.BasketCheckout.UserName).NotEmpty();
        RuleFor(x => x.BasketCheckout.CustomerId).NotEmpty();
        RuleFor(x => x.BasketCheckout.TotalPrice).GreaterThan(0);
        RuleFor(x => x.BasketCheckout.FirstName).NotEmpty();
        RuleFor(x => x.BasketCheckout.LastName).NotEmpty();
        RuleFor(x => x.BasketCheckout.EmailAddress).NotEmpty().EmailAddress();
        RuleFor(x => x.BasketCheckout.AddressLine).NotEmpty();
        RuleFor(x => x.BasketCheckout.Country).NotEmpty();
        RuleFor(x => x.BasketCheckout.State).NotEmpty();
        RuleFor(x => x.BasketCheckout.ZipCode).NotEmpty();
        RuleFor(x => x.BasketCheckout.CardName).NotEmpty();
        RuleFor(x => x.BasketCheckout.CardNumber).NotEmpty().CreditCard();
        RuleFor(x => x.BasketCheckout.Expiration).NotEmpty();
        RuleFor(x => x.BasketCheckout.CVV).NotEmpty().Length(3, 4);
        RuleFor(x => x.BasketCheckout.PaymentMethod).GreaterThan(0);
    }
}

public class CheckoutHandler(IBasketRepository repository, IPublishEndpoint publishEndpoint) : ICommandHandler<CheckoutBasketCommand, CheckoutBasketResult>
{
    public async Task<CheckoutBasketResult> Handle(CheckoutBasketCommand request, CancellationToken cancellationToken)
    {
        var basket = await repository.GetBasketAsync(request.BasketCheckout.UserName, cancellationToken);
        if(basket == null) return new CheckoutBasketResult(false);

        var eventMessage = request.BasketCheckout.Adapt<BasketCheckoutEvent>();
        eventMessage.TotalPrice = basket.TotalPrice;

        await publishEndpoint.Publish(eventMessage, cancellationToken);

        await repository.DeleteBasketAsync(basket.UserName, cancellationToken);

        return new CheckoutBasketResult(true);
    }
}
