using Carter.ModelBinding;

namespace Catalog.API.Products.CreateProduct;

public record CreateProductRequest(
    string Name,
    string Description,
    string ImageUrl,
    List<string> Categories,
    decimal Price);

public record CreateProductResponse(Guid Id);
public class CreatProductEndpoint : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapPost("/products", async (HttpRequest httpRequest, CreateProductRequest request, ISender mediator) =>
        {
            
            var command = request.Adapt<CreateProductCommand>();
            //var validationResult = httpRequest.Validate(command);
            //if (validationResult.IsValid is false)
            //{
            //    return Results.BadRequest(validationResult.Errors);
            //}

            var response = await mediator.Send(command);
            return Results.Created($"Product Created with Id {response.Id}", response.Adapt<CreateProductResponse>());
        }).WithName("CreateProduct")
        .Produces<CreateProductResponse>(StatusCodes.Status201Created)
        .Produces(StatusCodes.Status400BadRequest)
        .Produces(StatusCodes.Status500InternalServerError)
        .WithSummary("Create a new product")
        .WithDescription("This endpoint allows you to create a new product in the catalog. " +
                         "You need to provide the product details such as name, description, image URL, categories, and price.");

    }
}
