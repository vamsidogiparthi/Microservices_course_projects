namespace Ordering.Domain.Abstractions;

public abstract class Aggregate<TId>: Entity<TId>, IAggregate<TId>
    where TId : notnull
{
    private readonly List<IDomainEvent> _domainEvents = [];
    public IReadOnlyCollection<IDomainEvent> DomainEvents => _domainEvents.AsReadOnly();
    public void AddDomainEvent(IDomainEvent domainEvent)
    {
        ArgumentNullException.ThrowIfNull(domainEvent);
        _domainEvents.Add(domainEvent);

    }
    public IDomainEvent[] ClearDomainEvents()
    {
        var events = _domainEvents.ToArray();
        _domainEvents.Clear();
       
        return events;
    }
}
