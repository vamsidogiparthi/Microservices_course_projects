namespace Basket.API.Basket.GetBasket;
public record GetBasketQuery(string UserName) : IQuery<GetBasketQueryResult>;
public record GetBasketQueryResult(ShoppingCart ShoppingCart);
public class GetBasketQueryHandler(IBasketRepository db) : IQueryHandler<GetBasketQuery, GetBasketQueryResult>
{
    public async Task<GetBasketQueryResult> Handle(GetBasketQuery query, CancellationToken cancellationToken)
    {
        var basket = await db.GetBasketAsync(query.UserName, cancellationToken);

        return new GetBasketQueryResult(basket);
    }
}
