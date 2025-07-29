
namespace Catalog.API.Products.UpdateProduct;

public record UpdateProductRequest(Guid Id, string Name, string Description, decimal Price, string[] Categories);
public record UpdateProductResponse(bool IsSuccess);
public class UpdateProductEndpoint : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapPut("/products", async (UpdateProductRequest request, ISender sender) =>
        {
            var command = request.Adapt<UpdateProductCommand>();
            var response = await sender.Send(command);
            return response.IsSuccess
                ? Results.Ok(new UpdateProductResponse(true))
                : Results.BadRequest(new UpdateProductResponse(false));
        })
            .WithDescription("Update an existing product")
            .WithName("UpdateProduct")
            .Produces<UpdateProductResponse>(StatusCodes.Status200OK)
            .Produces<UpdateProductResponse>(StatusCodes.Status400BadRequest)
            .ProducesProblem(StatusCodes.Status500InternalServerError)
            .WithSummary("Update the details of an existing product in the catalog.");
    }
}
