
using BuildingBlocks.Pagination;

namespace Ordering.Application.Orders.Queries.GetOrders;

public record GetOrdersQuery(PaginationResult<OrderDto> Pagination) : IQuery<GetOrdersResult>;

public record GetOrdersResult(PaginationResult<OrderDto> Orders);
