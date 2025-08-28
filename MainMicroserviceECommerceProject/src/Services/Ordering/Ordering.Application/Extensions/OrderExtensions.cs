
namespace Ordering.Application.Extensions;

public static class OrderExtensions
{
    public static IEnumerable<OrderDto> ProjectOrderDtoList(this IEnumerable<Order> orders)
    {
        var orderDtos = orders.Select(static order => new OrderDto(
            Id: order.Id.Value,
            CustomerId: order.Customer.Value,
            OrderName: order.OrderName.Value,
            ShippingAddress: new AddressDto(
                FirstName: order.ShippingAddress.FirstName,
                Country: order.ShippingAddress.Country,
                LastName: order.ShippingAddress.LastName,
                EmailAddress: order.ShippingAddress.Email,
                AddressLine: order.ShippingAddress.AddressLine,
                State: order.ShippingAddress.State,
                ZipCode: order.ShippingAddress.ZipCode),
            BillingAddress: new AddressDto(
                 FirstName: order.ShippingAddress.FirstName,
                Country: order.BillingAddress.Country,
                LastName: order.BillingAddress.LastName,
                EmailAddress: order.BillingAddress.Email,
                AddressLine: order.BillingAddress.AddressLine,
                State: order.BillingAddress.State,
                ZipCode: order.BillingAddress.ZipCode),
            Payment: new PaymentDto(
                CardName: order.Payment.CardName ?? throw new ArgumentNullException(order.Payment.CardName),
                CardNumber: order.Payment.CardNumber,
                Expiration: order.Payment.Expiration,
                Cvv: order.Payment.CVV,
                PaymentType: order.Payment.PaymentMethod),
            Status: order.Status,
            OrderItems: [.. order.OrderItems.Select(item => new OrderItemDto(
                OrderId: item.Id.Value,
                ProductId: item.ProductId.Value,
                Price: item.Price,
                Quantity: item.Quantity)
            )]
        ));
        return orderDtos;
    }
}
