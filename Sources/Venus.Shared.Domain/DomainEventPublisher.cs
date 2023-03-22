using MediatR;

namespace Venus.Shared.Domain;

public sealed class DomainEventPublisher
{
    private readonly IPublisher _publisher;

    public DomainEventPublisher(IPublisher publisher)
    {
        ArgumentNullException.ThrowIfNull(publisher);
        _publisher = publisher;
    }

    public Task Publish<TDomainEvent>(TDomainEvent domainEvent, CancellationToken cancellationToken) 
        where TDomainEvent: DomainEvent
    {
        var notification = new DomainEventNotification<TDomainEvent>(domainEvent);
        return  _publisher.Publish(notification, cancellationToken);
    }
    
    public Task Publish<TDomainEvent>(IReadOnlyCollection<TDomainEvent> domainEvents, CancellationToken cancellationToken) 
        where TDomainEvent: DomainEvent
    {
        var notification = new DomainEventNotification<TDomainEvent>(domainEvents.ToArray());
        return  _publisher.Publish(notification, cancellationToken);
    }
}