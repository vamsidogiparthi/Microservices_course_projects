namespace Ordering.Domain.Models;

public class Order: Aggregate<OrderId>
{
    private readonly List<OrderItem> _orderItems = [];

    public IReadOnlyCollection<OrderItem> OrderItems => _orderItems.AsReadOnly();

    public CustomerId Customer { get; private set; } = default!;
    public OrderName OrderName { get; private set; } = default!;
    public Address ShippingAddress { get; private set; } = default!;
    public Address BillingAddress { get; private set; } = default!;
    public Payment Payment { get; private set; } = default!;
    public OrderStatus Status { get; private set; } = OrderStatus.Pending;
    public decimal TotalPrice
    {
        get
        {
            return _orderItems.Sum(item => item.Price * item.Quantity);
        }
    }

    public static Order Create(OrderId orderId, CustomerId customerId, OrderName orderName, Address shippingAddress, Address billingAddress, Payment payment)
    {        
        var order = new Order
        {
            Id = orderId,
            Customer = customerId,
            OrderName = orderName,
            ShippingAddress = shippingAddress,
            BillingAddress = billingAddress,
            Payment = payment
        };

        order.AddDomainEvent(new OrderCreatedEvent(order));

        return order;
    }

    public void Update(OrderName orderName, Address shippingAddress, Address billingAddress, Payment payment)
    {
        OrderName = orderName;
        ShippingAddress = shippingAddress;
        BillingAddress = billingAddress;
        Payment = payment;

        AddDomainEvent(new OrderUpdatedEvent(this));
    }

    public void Add(ProductId productId, decimal price, int quantity)
    {        
        ArgumentOutOfRangeException.ThrowIfNegativeOrZero(price);
        ArgumentOutOfRangeException.ThrowIfNegative(quantity);
        var orderItem = new OrderItem(Id, productId, quantity, price);
        _orderItems.Add(orderItem);        
    }

    public void Remove(OrderItemId orderItemId)
    {
        var orderItem = _orderItems.FirstOrDefault(item => item.Id == orderItemId) ?? 
            throw new InvalidOperationException($"Order item with id {orderItemId} not found in order {Id}");
        _orderItems.Remove(orderItem);        
    }

}
