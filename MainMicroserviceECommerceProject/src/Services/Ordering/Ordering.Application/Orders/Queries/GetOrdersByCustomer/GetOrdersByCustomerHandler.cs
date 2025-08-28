

namespace Ordering.Application.Orders.Queries.GetOrdersByCustomer;

public class GetOrdersByCustomerHandler(IApplicationDbContext dbContext) : IQueryHandler<GetOrderByCustomerQuery, GetOrdersByCustomerResult>
{
    public async Task<GetOrdersByCustomerResult> Handle(GetOrderByCustomerQuery query, CancellationToken cancellationToken)
    {

        var orders = await dbContext.Orders.Where(Orders => Orders.Customer == CustomerId.Of(query.CustomerId))
            .Include(orders => orders.OrderItems)
            .AsNoTracking()
            .ToListAsync(cancellationToken: cancellationToken);

        if (orders.Count == 0)
        {
            return new GetOrdersByCustomerResult([]);
        }

        var orderDtos = orders.ProjectOrderDtoList();        
        return new GetOrdersByCustomerResult(orderDtos);

    }
}
