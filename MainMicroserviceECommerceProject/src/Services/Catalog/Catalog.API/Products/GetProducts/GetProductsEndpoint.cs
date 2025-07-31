namespace Catalog.API.Products.GetProducts;

public record GetProductsRequest(int? PageNumber = 1, int? PageSize = 10);
public record GetProductsResponse(IEnumerable<Product> Products);
public class GetProductsEndpoint : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapGet("/products", async ([AsParameters] GetProductsRequest request, ISender mediator) =>
        {
            var query = request.Adapt<GetProductsQuery>();
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
