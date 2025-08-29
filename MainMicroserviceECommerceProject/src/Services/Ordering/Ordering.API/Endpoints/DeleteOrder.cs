namespace Ordering.API.Endpoints;

public record DeleteOrderResult(bool IsDeleted);
public class DeleteOrder : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapDelete("/orders/{id:guid}", async (Guid id, ISender sender) =>
        {
            var result = await sender.Send(new DeleteOrderCommand(id));
            return Results.Ok(result.Adapt<DeleteOrderResult>());
        }).WithName("Delete Order")
        .WithDescription("Deletes an existing order")
        .Produces<DeleteOrderResult>(StatusCodes.Status200OK)
        .ProducesProblem(statusCode: StatusCodes.Status400BadRequest)
        .ProducesProblem(statusCode: StatusCodes.Status404NotFound);
    }
}
