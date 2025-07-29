
using Catalog.API.Models;

namespace Catalog.API.Products.GetProductByCategory;

//public record GetProductByCategoryRequest()
public record GetProductByCategoryResponse(IEnumerable<Product> Products);

public class GetProductByCategoryEndpoint : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapGet("/products/category/{category}", async (string category, ISender mediator) =>
        {
            var query = new GetProductByCategoryQuery(category);
            var response = await mediator.Send(query);
            return Results.Ok(response.Adapt<GetProductByCategoryResponse>());
        }).WithName("GetProductByCategory")
          .WithDescription("Get products by category")
          .Produces<GetProductByCategoryResponse>(StatusCodes.Status200OK)
          .ProducesProblem(StatusCodes.Status404NotFound)
          .ProducesProblem(StatusCodes.Status500InternalServerError)
          .WithSummary("Retrieve products by their category.");
    }
}
