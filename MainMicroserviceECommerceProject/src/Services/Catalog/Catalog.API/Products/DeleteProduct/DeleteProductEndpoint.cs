
namespace Catalog.API.Products.DeleteProduct;

// public record DeleteProductRequest 
public record DeleteProductResponse(bool IsSuccess);
public class DeleteProductEndpoint : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapDelete("/products/{productId:guid}", async (Guid productId, ISender sender) =>
        {
            var result = await sender.Send(new DeleteProductCommand(productId));
            return Results.Ok(result.Adapt<DeleteProductResponse>());
        }).WithName("DeleteProduct")
            .WithDescription("Delete a product by its ID")
            .Produces<DeleteProductResponse>(StatusCodes.Status200OK)
            .ProducesProblem(StatusCodes.Status404NotFound)
            .ProducesProblem(StatusCodes.Status500InternalServerError)
            .WithSummary("Delete a product from the catalog by its unique identifier.")
            .WithTags("Products");
    }
}
