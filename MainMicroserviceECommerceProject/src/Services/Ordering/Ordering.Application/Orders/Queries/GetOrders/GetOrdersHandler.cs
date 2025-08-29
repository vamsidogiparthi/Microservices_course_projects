



namespace Ordering.Application.Orders.Queries.GetOrders;

public class GetOrdersHandler(IApplicationDbContext dbContext) : IQueryHandler<GetOrdersQuery, GetOrdersResult>
{
    public async Task<GetOrdersResult> Handle(GetOrdersQuery query, CancellationToken cancellationToken)
    {
        var totalCount = await dbContext.Orders.LongCountAsync(cancellationToken: cancellationToken);

        var currentPageQuery = await dbContext.Orders
            .Include(orders => orders.OrderItems)
            .AsNoTracking()
            .Skip((query.PaginationRequest.PageIndex - 1) * query.PaginationRequest.PageSize)
            .Take(query.PaginationRequest.PageSize)
            .ToListAsync(cancellationToken);

        return new GetOrdersResult(new PaginationResult<OrderDto>(
            query.PaginationRequest.PageIndex,
            query.PaginationRequest.PageSize,
            totalCount,
            currentPageQuery.ProjectOrderDtoList()));
    }
}
