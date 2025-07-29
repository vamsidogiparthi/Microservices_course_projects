using BuildingBlocks.CQRS;
using Catalog.API.Models;
using Marten;

namespace Catalog.API.Products.DeleteProduct;

public record DeleteProductCommand(Guid ProductId): ICommand<DeleteProductResult>;
public record DeleteProductResult(bool IsSuccess);
public class DeleteProductHandler(ILogger<DeleteProductHandler> logger, IDocumentSession db) : ICommandHandler<DeleteProductCommand, DeleteProductResult>
{
    public async Task<DeleteProductResult> Handle(DeleteProductCommand command, CancellationToken cancellationToken)
    {
       logger.LogInformation("Handling DeleteProductCommand for ProductId: {ProductId}", command.ProductId);
        var product = await db.LoadAsync<Product>(command.ProductId, cancellationToken);
        if (product == null)
        {
            logger.LogWarning("Product with Id {ProductId} not found", command.ProductId);
            return new DeleteProductResult(false);
        }
        db.Delete(product);
        await db.SaveChangesAsync(cancellationToken);
        logger.LogInformation("Product with Id {ProductId} deleted successfully", command.ProductId);
        return new DeleteProductResult(true);
    }
}
