

namespace Catalog.API.Products.GetProductById;

public record GetProductByIdResponse(Product Product);


public class GetProductByIdEndpoint : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapGet("/products/{id:guid}", async (Guid id, ISender mediator) =>
        {
            var query = new GetProductByIdQuery(id);
            var response = await mediator.Send(query);
            return Results.Ok(response.Adapt<GetProductByIdResponse>());
        }).WithName("GetProductById")
        .WithDescription("Get a product by its ID")
        .Produces<GetProductByIdResponse>(StatusCodes.Status200OK)
        .ProducesProblem(StatusCodes.Status404NotFound)
        .ProducesProblem(StatusCodes.Status500InternalServerError)
        .WithSummary("Retrieve a product by its unique identifier.");

    }
}
