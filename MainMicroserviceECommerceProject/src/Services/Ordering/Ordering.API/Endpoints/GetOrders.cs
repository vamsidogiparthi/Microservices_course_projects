


namespace Ordering.API.Endpoints;

public record GetOrdersResult(PaginationResult<OrderDto> Orders);
public class GetOrders : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapGet("/orders", async ([AsParameters] PaginationRequest request, ISender sender) =>
        {
            var query = new GetOrdersQuery(request);
            var result = await sender.Send(query);
            return Results.Ok(result.Adapt<GetOrdersResult>());
        }).WithName("GetOrders")
        .WithDescription("Get all orders")
        .Produces(StatusCodes.Status200OK)
        .ProducesProblem(StatusCodes.Status400BadRequest)
        .ProducesProblem(StatusCodes.Status404NotFound);

    }
}
