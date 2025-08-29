
namespace Ordering.Application.Orders.Commands.UpdateOrder;

public class UpdateOrdeHandler(IApplicationDbContext dbContext) : ICommandHandler<UpdateOrderCommand, UpdateOrderResult>
{
    public async Task<UpdateOrderResult> Handle(UpdateOrderCommand command, CancellationToken cancellationToken)
    {
        var existingOrder = await dbContext.Orders.Include(o => o.OrderItems)
            .FirstOrDefaultAsync(o => o.Id == OrderId.Of(command.Order.Id), cancellationToken) ?? throw new OrderNotFoundException(command.Order.Id);
        
        UpdateOrder(command.Order, existingOrder);

        dbContext.Orders.Update(existingOrder);
        await dbContext.SaveChangesAsync(cancellationToken);

        return new UpdateOrderResult(true);
    }

    private static void UpdateOrder(OrderDto order, Order existingOrder)
    { 
        var shippingAddress = Address.Of(firstName: order.ShippingAddress.FirstName, lastName: order.ShippingAddress.LastName,
            email: order.ShippingAddress.EmailAddress, addressLine: order.ShippingAddress.AddressLine,
            country: order.ShippingAddress.Country, state: order.ShippingAddress.State, zipCode: order.ShippingAddress.ZipCode);
        var billingAddress = Address.Of(firstName: order.BillingAddress.FirstName, lastName: order.BillingAddress.LastName,
            email: order.BillingAddress.EmailAddress, addressLine: order.BillingAddress.AddressLine,
            country: order.BillingAddress.Country, state: order.BillingAddress.State, zipCode: order.BillingAddress.ZipCode);
        var payment = Payment.Of(order.Payment.CardName, order.Payment.CardNumber, order.Payment.Expiration, order.Payment.Cvv, order.Payment.PaymentType);


        existingOrder.Update(OrderName.Of(order.OrderName), shippingAddress, billingAddress, payment);
    }
}
