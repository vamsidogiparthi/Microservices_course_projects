
namespace Basket.API.Basket.CheckoutBasket;

public record CheckoutBasketRequest(BasketCheckoutDto BasketCheckout);
public record CheckoutBasketResponse(bool IsSuccess);
public class CheckoutBasketEndpoints : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapPost("/basket/checkout", async (CheckoutBasketRequest request, ISender sender) =>
        {
            var command = request.Adapt<CheckoutBasketCommand>();
            var result = await sender.Send(command);
            return Results.Ok(result.Adapt<CheckoutBasketResponse>());
        }).WithName("CheckoutBasket")
        .WithDescription("Checkout the current basket")
        .Produces<CheckoutBasketResponse>(StatusCodes.Status202Accepted)
        .ProducesProblem(StatusCodes.Status400BadRequest)
        .ProducesProblem(StatusCodes.Status500InternalServerError)
        .WithSummary("Checkout the current user's basket and initiate the order process.");
    }
}
