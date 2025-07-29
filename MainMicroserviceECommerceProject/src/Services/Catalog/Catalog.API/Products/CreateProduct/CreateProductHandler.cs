using Catalog.API.Models;

namespace Catalog.API.Products.CreateProduct;

public record CreateProductCommand(
    string Name,
    string Description,
    string ImageUrl,
    List<string> Categories,
    decimal Price): ICommand<CreateProductResult>;

public record CreateProductResult(Guid Id);
//public class CreateProductCommandValidator : AbstractValidator<CreateProductCommand>
//{
//    public CreateProductCommandValidator()
//    {
//        RuleFor(x => x.Name).NotEmpty().WithMessage("Product name is required.");
//        RuleFor(x => x.Description).NotEmpty().WithMessage("Product description is required.");
//        RuleFor(x => x.ImageUrl).NotEmpty().WithMessage("Product image URL is required.");
//        RuleFor(x => x.Categories).NotEmpty().WithMessage("At least one category is required.");
//        RuleFor(x => x.Price).GreaterThan(0).WithMessage("Price must be greater than zero.");
//    }
//}
public class CreateProductHandler(IDocumentSession dbSession) : ICommandHandler<CreateProductCommand, CreateProductResult>
{
    public async Task<CreateProductResult> Handle(CreateProductCommand command, CancellationToken cancellationToken)
    {
        // Create a product entity
        //var validationResult = await validator.ValidateAsync(command, cancellationToken);

        //if (validationResult.IsValid is false)
        //{
        //    throw new ValidationException(validationResult.Errors);
        //}

        var product = new Product
        {
            Name = command.Name,
            Description = command.Description,
            ImageUrl = command.ImageUrl,
            Categories = command.Categories,
            Price = command.Price
        };
        // Save to Db.
        dbSession.Store(product);
        await dbSession.SaveChangesAsync(cancellationToken);
        // return Create Product Result result.

        return new CreateProductResult(product.Id);
    }
}