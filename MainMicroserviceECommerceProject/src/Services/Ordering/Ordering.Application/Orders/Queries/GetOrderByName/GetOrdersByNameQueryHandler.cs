


namespace Ordering.Application.Orders.Queries.GetOrderByName;

public class GetOrdersByNameQueryHandler(IApplicationDbContext dbContext) : IQueryHandler<GetOrdersByNameQuery, GetOrdersByNameResult>
{
    public async Task<GetOrdersByNameResult> Handle(GetOrdersByNameQuery query, CancellationToken cancellationToken)
    {
        var orders = await dbContext.Orders.Where(Orders => Orders.OrderName.Value == query.OrderName)
            .Include(orders => orders.OrderItems)
            .AsNoTracking()
            .ToListAsync(cancellationToken: cancellationToken);
        
        var orderDtos = orders.ProjectOrderDtoList();        
        return new GetOrdersByNameResult(Orders: orderDtos);
    }
}
