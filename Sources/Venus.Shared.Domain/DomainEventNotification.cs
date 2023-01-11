using MediatR;

namespace Venus.Shared.Domain;

public class DomainEventNotification<TDomainEvent>: INotification 
    where TDomainEvent: DomainEvent
{
    public DomainEventNotification(params TDomainEvent[] domainEvents)
    {
        ArgumentNullException.ThrowIfNull(domainEvents);
        DomainEvents = domainEvents;
    }
    
    public IReadOnlyCollection<TDomainEvent> DomainEvents { get; }
}