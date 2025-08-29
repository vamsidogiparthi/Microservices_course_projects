
namespace Ordering.Application.Orders.Queries.GetOrderByName;

public record GetOrdersByNameQuery(string OrderName) : IQuery<GetOrdersByNameResult>;
public record GetOrdersByNameResult(IEnumerable<OrderDto> Orders);



