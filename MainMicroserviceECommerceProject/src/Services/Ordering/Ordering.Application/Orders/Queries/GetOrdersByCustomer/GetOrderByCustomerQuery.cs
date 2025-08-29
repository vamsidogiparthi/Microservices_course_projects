
namespace Ordering.Application.Orders.Queries.GetOrdersByCustomer;

public record GetOrderByCustomerQuery(Guid CustomerId) : IQuery<GetOrdersByCustomerQueryResult>;
public record GetOrdersByCustomerQueryResult(IEnumerable<OrderDto> Orders);
