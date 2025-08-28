

namespace Ordering.Application.Orders.Commands.DeleteOrder;

public class DeleteOrderHandler(IApplicationDbContext dbContext) : ICommandHandler<DeleteOrderCommand, DeleteOrderResult>
{
    public async Task<DeleteOrderResult> Handle(DeleteOrderCommand command, CancellationToken cancellationToken)
    {
        var existingOrder = await dbContext.Orders
            .FindAsync([OrderId.Of(command.Id)], cancellationToken) ?? throw new OrderNotFoundException(command.Id);

        dbContext.Orders.Remove(existingOrder);
        await dbContext.SaveChangesAsync(cancellationToken);

        return new DeleteOrderResult(true);
    }
}
