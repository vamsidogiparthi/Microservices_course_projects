



namespace Ordering.Application.Orders.Queries.GetOrders;

public class GetOrdersHandler(IApplicationDbContext dbContext) : IQueryHandler<GetOrdersQuery, GetOrdersResult>
{
    public async Task<GetOrdersResult> Handle(GetOrdersQuery query, CancellationToken cancellationToken)
    {
        var totalCount = await dbContext.Orders.LongCountAsync(cancellationToken: cancellationToken);

        var currentPageQuery = await dbContext.Orders
            .Include(orders => orders.OrderItems)
            .AsNoTracking()
            .Skip((query.Pagination.PageIndex - 1) * query.Pagination.PageSize)
            .Take(query.Pagination.PageSize)
            .ToListAsync(cancellationToken);

        return new GetOrdersResult(new PaginationResult<OrderDto>(
            query.Pagination.PageIndex,
            query.Pagination.PageSize,
            totalCount,
            currentPageQuery.ProjectOrderDtoList()));
    }
}
