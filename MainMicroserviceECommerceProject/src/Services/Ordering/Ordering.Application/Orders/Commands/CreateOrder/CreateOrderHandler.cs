
namespace Ordering.Application.Orders.Commands.CreateOrder;

public class CreateOrderHandler(IApplicationDbContext dbContext) : ICommandHandler<CreateOrderCommand, CreateOrderResult>
{
    public async Task<CreateOrderResult> Handle(CreateOrderCommand command, CancellationToken cancellationToken)
    {
        // Todo: Create Order Entity from command object
        var order = CreateNewOrder(command.Order);
        // Save to Database.
        dbContext.Orders.Add(order);
        await dbContext.SaveChangesAsync(cancellationToken);

        // Return Result.        
        return new CreateOrderResult(order.Id.Value);
    }

    private static Order CreateNewOrder(OrderDto order)
    {
        var newOrder = Order.Create(
            orderId: OrderId.Of(Guid.NewGuid()), 
            customerId: CustomerId.Of(order.CustomerId), 
            orderName: OrderName.Of(order.OrderName),
            shippingAddress: Address.Of(order.ShippingAddress.Country, order.ShippingAddress.LastName,
            order.ShippingAddress.EmailAddress, order.ShippingAddress.AddressLine,
            order.ShippingAddress.Country, order.ShippingAddress.State, order.ShippingAddress.ZipCode),
            billingAddress: Address.Of(order.BillingAddress.Country, order.BillingAddress.LastName,
            order.BillingAddress.EmailAddress, order.BillingAddress.AddressLine,
            order.BillingAddress.Country, order.BillingAddress.State, order.BillingAddress.ZipCode),
            payment: Payment.Of(order.Payment.CardName, order.Payment.CardNumber,order.Payment.Expiration, order.Payment.Cvv, order.Payment.PaymentType)
            );

        foreach(var item in order.OrderItems)
        {
            newOrder.Add(ProductId.Of(item.ProductId), item.Price, item.Quantity);
        }

        return newOrder;
    }
}
