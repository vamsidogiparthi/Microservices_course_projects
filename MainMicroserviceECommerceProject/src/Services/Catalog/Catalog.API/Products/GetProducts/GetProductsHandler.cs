using BuildingBlocks.CQRS;
using Marten;
using Catalog.API.Models;

namespace Catalog.API.Products.GetProducts;

public record GetProductsQuery() : IQuery<GetProductsResult>;
public record GetProductsResult(IEnumerable<Product> Products);
public class GetProductsHandler(IQuerySession querySession): IQueryHandler<GetProductsQuery, GetProductsResult>
{
    private readonly IQuerySession _dbSession = querySession;
    public async Task<GetProductsResult> Handle(GetProductsQuery query, CancellationToken cancellationToken)
    {
        var products = await _dbSession.Query<Product>()
            .ToListAsync(token: cancellationToken);

        return new GetProductsResult(products);
    }
}

