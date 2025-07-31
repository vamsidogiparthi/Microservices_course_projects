using BuildingBlocks.CQRS;
using Catalog.API.Models;
using Marten;

namespace Catalog.API.Products.UpdateProduct
{
    public record UpdateProductCommand(Guid Id, string Name, string Description, decimal Price, string[] Categories) : ICommand<UpdateProductResult>;
    public record UpdateProductResult(bool IsSuccess);

    public class UpdateCommandHandlerValidator: AbstractValidator<UpdateProductCommand>
    {
        public UpdateCommandHandlerValidator()
        {
            RuleFor(x => x.Id).NotEmpty().WithMessage("Product Id is required.");
            RuleFor(x => x.Name).NotEmpty().WithMessage("Product name is required.")
                .Length(2, 150).WithMessage("Length of the name should be between 2 to 150 characters.");
            RuleFor(x => x.Description).NotEmpty().WithMessage("Product description is required.");
            RuleFor(x => x.Price).GreaterThan(0).WithMessage("Price must be greater than zero.");
            RuleFor(x => x.Categories).NotEmpty().WithMessage("At least one category is required.");
        }
    }
    public class UpdateProductCommandHandler(IDocumentSession db, ILogger<UpdateProductCommandHandler> logger) : ICommandHandler<UpdateProductCommand, UpdateProductResult>
    {
        public async Task<UpdateProductResult> Handle(UpdateProductCommand commad, CancellationToken cancellationToken)
        {
            logger.LogInformation("Handling UpdateProductCommand for product with Id: {Id}", commad.Id);
            // Retrieve the product from the database
            var product = (await db.LoadAsync<Product>(commad.Id, cancellationToken) ?? throw new KeyNotFoundException($"Product with Id = {commad.Id} is not found.")) ?? throw new ProductNotFoundException(nameof(Product), commad.Id);

            product.Name = commad.Name;
            product.Description = commad.Description;
            product.Price = commad.Price;
            product.Categories = commad.Categories.ToList();
            db.Update(product);
            await db.SaveChangesAsync(cancellationToken);

            return new UpdateProductResult(true); ;
        }
    }
}
