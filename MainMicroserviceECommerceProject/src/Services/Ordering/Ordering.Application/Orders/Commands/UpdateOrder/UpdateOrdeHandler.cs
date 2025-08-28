
namespace Ordering.Application.Orders.Commands.UpdateOrder;

public class UpdateOrdeHandler(IApplicationDbContext dbContext) : ICommandHandler<UpdateOrderCommand, UpdateOrderResult>
{
    public async Task<UpdateOrderResult> Handle(UpdateOrderCommand command, CancellationToken cancellationToken)
    {
        var existingOrder = await dbContext.Orders
            .FindAsync([OrderId.Of(command.Order.Id)], cancellationToken) ?? throw new OrderNotFoundException(command.Order.Id);
        
        UpdateOrder(command.Order, existingOrder);

        dbContext.Orders.Update(existingOrder);
        await dbContext.SaveChangesAsync(cancellationToken);

        return new UpdateOrderResult(true);
    }

    private static void UpdateOrder(OrderDto order, Order existingOrder)
    { 
        var shippingAddress = Address.Of(order.ShippingAddress.Country, order.ShippingAddress.LastName,
            order.ShippingAddress.EmailAddress, order.ShippingAddress.AddressLine,
            order.ShippingAddress.Country, order.ShippingAddress.State, order.ShippingAddress.ZipCode);
        var billingAddress = Address.Of(order.BillingAddress.Country, order.BillingAddress.LastName,
            order.BillingAddress.EmailAddress, order.BillingAddress.AddressLine,
            order.BillingAddress.Country, order.BillingAddress.State, order.BillingAddress.ZipCode);
        var payment = Payment.Of(order.Payment.CardName, order.Payment.CardNumber, order.Payment.Expiration, order.Payment.Cvv, order.Payment.PaymentType);


        existingOrder.Update(OrderName.Of(order.OrderName), shippingAddress, billingAddress, payment);
    }
}
