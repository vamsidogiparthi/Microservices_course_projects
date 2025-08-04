using Mapster;

namespace Basket.API.Basket.StoreBasket;

public record StoreBasketRequest(ShoppingCart ShoppingCart);
public record StoreBasketResponse(string UserName);

public class StoreBasketEndpoints : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapPost("/basket", async (StoreBasketRequest request, ISender send) =>
        {
            var result = await send.Send(request.Adapt<StoreBasketCommand>());
            return Results.Created($"Cart created for {result.Adapt<StoreBasketResponse>().UserName}", result);
        })
        .WithName("StoreBasket")
        .WithTags("Basket")
        .WithDescription("Store a user's shopping basket")
        .Produces<StoreBasketResponse>(StatusCodes.Status200OK)
        .Produces(StatusCodes.Status400BadRequest)
        .Produces(StatusCodes.Status500InternalServerError);


    }
}
