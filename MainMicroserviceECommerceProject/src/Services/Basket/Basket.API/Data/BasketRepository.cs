
using Basket.API.Exceptions;
using Marten;

namespace Basket.API.Data;

public class BasketRepository(IDocumentSession db) : IBasketRepository
{
    public async Task<bool> DeleteBasketAsync(string userName, CancellationToken cancellationToken = default)
    {
       var basket = await db.LoadAsync<ShoppingCart>(userName, cancellationToken);
        if (basket == null)
        {
            return false;
        }
        db.Delete(basket);
        await db.SaveChangesAsync(cancellationToken);
        return true;

    }

    public async Task<ShoppingCart> GetBasketAsync(string userName, CancellationToken cancellationToken = default)
    {
        var basket = await db.LoadAsync<ShoppingCart>(userName, cancellationToken);

        return basket ?? throw new BasketNotFoundException(userName);

    }

    public async Task<ShoppingCart> StoreBasketAsync(ShoppingCart cart, CancellationToken cancellationToken = default)
    {
        db.Store(cart);
        await db.SaveChangesAsync(cancellationToken);

        return cart;
    }
}
