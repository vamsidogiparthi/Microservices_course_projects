using MediatR;

namespace Ordering.Infrastructure.Data.Interceptors;

public class DispatchDomainEventInterceptor(IMediator mediator): SaveChangesInterceptor
{

    public override InterceptionResult<int> SavingChanges(DbContextEventData eventData, InterceptionResult<int> result)
    {
        DispatchDomainEvents(eventData.Context).GetAwaiter().GetResult();
        return base.SavingChanges(eventData, result);
    }

    public override async ValueTask<InterceptionResult<int>> SavingChangesAsync(DbContextEventData eventData, InterceptionResult<int> result, CancellationToken cancellationToken = default)
    {
        await DispatchDomainEvents(eventData.Context);
        return await base.SavingChangesAsync(eventData, result, cancellationToken);
    }


    private async Task DispatchDomainEvents(DbContext? dbContext)
    {
        if (dbContext is null) return;

        var aggregates = dbContext.ChangeTracker.Entries<IAggregate>().Where(a => a.Entity.DomainEvents.Count != 0).Select(a => a.Entity);

        var domainEvents = aggregates.SelectMany(a => a.DomainEvents).ToList(); // flattens the collection of collections into a single collection.
        var domainEvents1 = aggregates.Select(a => a.DomainEvents).ToList(); // returns the collection of sub collection.

        foreach (var item in aggregates)
        {
            item.ClearDomainEvents();
        }

        foreach (var item in domainEvents) // dispatch domain event
        {
            await mediator.Publish(item);
        }

    }
}
