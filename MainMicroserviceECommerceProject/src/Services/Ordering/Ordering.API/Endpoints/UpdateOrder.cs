

namespace Ordering.API.Endpoints;

public record UpdateOrderRequest(OrderDto Order);
public record UpdateOrderResponse(bool IsUpdated);

public class UpdateOrder : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapPut("/orders", async (UpdateOrderRequest request, ISender sender) =>
        {
            var result = await sender.Send(request.Adapt<UpdateOrderCommand>());
            return Results.Ok(result.Adapt<UpdateOrderResult>());
        }).WithName("UpdateOrder")
        .WithDescription("Updates an existing order")
        .Produces<UpdateOrderResponse>(StatusCodes.Status200OK) 
        .ProducesProblem(statusCode: StatusCodes.Status400BadRequest)
        .ProducesProblem(statusCode: StatusCodes.Status404NotFound)
        .ProducesProblem(statusCode: StatusCodes.Status500InternalServerError);
    }
}
