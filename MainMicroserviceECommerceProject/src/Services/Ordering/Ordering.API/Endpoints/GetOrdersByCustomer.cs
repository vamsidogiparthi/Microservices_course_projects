


namespace Ordering.API.Endpoints;

public record GetOrdersByCustomerResult(IEnumerable<OrderDto> Orders);
public class GetOrdersByCustomer : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapGet("/orders/customer/{customerId:guid}", async (Guid customerId, ISender sender) =>
        {
            var getOrdersByCustomerQuery = new GetOrderByCustomerQuery(customerId);
            var result = await sender.Send(getOrdersByCustomerQuery);

            return Results.Ok(result.Adapt<GetOrdersByCustomerResult>());
        }).WithName("Get Orders by Customer Id")
        .WithDescription("Get Order by Customer Id")
        .Produces(StatusCodes.Status200OK)
        .ProducesProblem(StatusCodes.Status400BadRequest)
        .ProducesProblem(StatusCodes.Status404NotFound);
    }
}
