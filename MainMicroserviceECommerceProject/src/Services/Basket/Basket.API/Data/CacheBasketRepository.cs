
using Microsoft.Extensions.Caching.Distributed;
using System.Text.Json;

namespace Basket.API.Data;

public class CacheBasketRepository(IBasketRepository repository, IDistributedCache cache) : IBasketRepository
{
   
    public async Task<ShoppingCart> GetBasketAsync(string userName, CancellationToken cancellationToken = default)
    {
        var cachedBasket = await cache.GetStringAsync(userName, cancellationToken);
        if (!string.IsNullOrEmpty(cachedBasket)) return JsonSerializer.Deserialize<ShoppingCart>(cachedBasket)!;

        var getBaskedFromDb = await repository.GetBasketAsync(userName, cancellationToken);
        await cache.SetStringAsync(userName, JsonSerializer.Serialize(getBaskedFromDb), cancellationToken);

        return getBaskedFromDb;
    }

    public async Task<ShoppingCart> StoreBasketAsync(ShoppingCart cart, CancellationToken cancellationToken = default)
    {
        var storedBasket = await repository.StoreBasketAsync(cart, cancellationToken);
        await cache.SetStringAsync(cart.UserName, JsonSerializer.Serialize(storedBasket), cancellationToken);

        return cart;
    }

    public async Task<bool> DeleteBasketAsync(string userName, CancellationToken cancellationToken = default)
    {
        var result = await repository.DeleteBasketAsync(userName, cancellationToken);
        if(result)
        {
            await cache.RemoveAsync(userName, cancellationToken);
        }

        return result;

    }

}
