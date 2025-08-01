﻿namespace Catalog.API.Products.GetProductById;

using System.Threading;
using System.Threading.Tasks;

public record GetProductByIdQuery(Guid Id): IQuery<GetProductByIdQueryResponse>;

public record GetProductByIdQueryResponse(Product Product);

public class GetProductByIdHandler(IQuerySession db) : IQueryHandler<GetProductByIdQuery, GetProductByIdQueryResponse>
{
    public async Task<GetProductByIdQueryResponse> Handle(GetProductByIdQuery query, CancellationToken cancellationToken)
    {
        var product = await db.Query<Product>()
            .FirstOrDefaultAsync(x => x.Id == query.Id, cancellationToken) ?? throw new KeyNotFoundException($"Product with Id {query.Id} not found.");

        return product == null ? throw new ProductNotFoundException(nameof(Product), query.Id) : new GetProductByIdQueryResponse(product);
    }
}
