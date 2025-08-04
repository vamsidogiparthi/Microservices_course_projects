
using Mapster;
using MediatR;

namespace Basket.API.Basket.GetBasket;

//public record GetBasketRequest(string UserName);
public record GetBasketResponse(ShoppingCart ShoppingCart);
public class GetBasketEndpoints : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapGet("/basket/{userName}", async (string userName, ISender sender) =>
        {
            var query = new GetBasketQuery(userName);
            var result = await sender.Send(query);
            var response = result.Adapt<GetBasketResponse>();
            return Results.Ok(response);
        })
        .WithName("GetBasket")
        .WithTags("Basket")
        .WithDescription("Get a user's shopping basket")
        .Produces<GetBasketResponse>(StatusCodes.Status200OK)
        .ProducesProblem(StatusCodes.Status404NotFound)
        .ProducesProblem(StatusCodes.Status500InternalServerError);
    }
}
