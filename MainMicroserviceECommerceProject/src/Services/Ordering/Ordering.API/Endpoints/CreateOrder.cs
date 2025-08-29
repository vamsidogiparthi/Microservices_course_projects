namespace Ordering.API.Endpoints;

// recieves create order request and communicates with application layer to create order

public record class CreateOrderRequest(OrderDto Order);
public record class CreateOrderResponse(Guid OrderId);

public class CreateOrder : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapPost("/orders", async (CreateOrderRequest request, ISender sender) =>
        {
            var result = await sender.Send(request.Adapt<CreateOrderCommand>());
            return Results.Created($"/orders/{result.Id}", new CreateOrderResponse(result.Id));
        }).WithName("Create Order")
        .WithDescription("Creates order")
        .Produces(statusCode: StatusCodes.Status201Created)
        .ProducesProblem(statusCode: StatusCodes.Status400BadRequest);
    }
}
