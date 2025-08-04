
using Mapster;

namespace Basket.API.Basket.DeleteBasket;

public record DeleteBasketResponse(bool IsSuccess);
public class DeleteBasketEndpoints : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapDelete("/basket/{userName}", async (string userName, ISender sender) =>
        {
            var command = new DeleteBasketCommand(userName);
            var result = await sender.Send(command);
            return Results.Ok(result.Adapt<DeleteBasketResponse>());
        })
        .WithName("DeleteBasket")
        .WithTags("Basket")
        .WithDescription("Delete a user's shopping basket")
        .Produces<DeleteBasketResponse>(StatusCodes.Status200OK)
        .ProducesProblem(StatusCodes.Status404NotFound)
        .ProducesProblem(StatusCodes.Status500InternalServerError);
    }
}
