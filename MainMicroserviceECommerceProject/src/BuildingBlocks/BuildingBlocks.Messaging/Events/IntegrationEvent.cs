﻿namespace BuildingBlocks.Messaging.Events;

public record IntegrationEvent
{
    public Guid Id { get; init; } = Guid.NewGuid();
    public DateTime OccuredOn { get; init; } = DateTime.UtcNow;
    public string EventType => GetType().AssemblyQualifiedName;
}
