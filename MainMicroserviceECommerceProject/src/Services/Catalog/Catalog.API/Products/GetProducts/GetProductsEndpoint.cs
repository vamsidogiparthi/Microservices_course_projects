
using Catalog.API.Models;

namespace Catalog.API.Products.GetProducts;

public record GetProductsResponse(IEnumerable<Product> Products);
public class GetProductsEndpoint : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapGet("/products", async (ISender mediator) =>
        {
            var query = new GetProductsQuery();
            var response = await mediator.Send(query);
            return Results.Ok(response.Adapt<GetProductsResponse>());
        }).WithDescription("Get all products")
          .WithName("GetProducts")
          .Produces<IEnumerable<GetProductsResult>>(StatusCodes.Status200OK)
          .ProducesProblem(StatusCodes.Status400BadRequest)
          .ProducesProblem(StatusCodes.Status500InternalServerError)
          .WithSummary("Retrieve all products from the catalog.")
          .WithDescription("This endpoint retrieves a list of all products available in the catalog.");
    }
}
