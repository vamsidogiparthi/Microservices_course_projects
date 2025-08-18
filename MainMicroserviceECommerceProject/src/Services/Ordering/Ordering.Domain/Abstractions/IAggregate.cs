namespace Ordering.Domain.Abstractions;


public interface IAggregate<TKey> : IEntity<TKey>, IAggregate
{
    // This interface inherits from IEntity<TKey> and IAggregate
    // It can be used to define aggregates with a specific key type
}

public interface IAggregate: IEntity
{
    IReadOnlyCollection<IDomainEvent> DomainEvents { get; }

    void AddDomainEvent(IDomainEvent domainEvent);

    IDomainEvent[] ClearDomainEvents();
}
