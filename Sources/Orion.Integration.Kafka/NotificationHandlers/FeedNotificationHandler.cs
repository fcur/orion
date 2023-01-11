using System.IO.Pipes;
using MediatR;
using Orion.App.Domain.FeedEntity.Events;
using Orion.Integration.Kafka.IntegrationEvents;
using Venus.Shared.Domain;
using FeedCreationDomainEvent = Orion.App.Domain.FeedEntity.Events.FeedCreationEvent;
using FeedCreationIntegrationEvent = Orion.Integration.Kafka.IntegrationEvents.FeedCreationEvent;

namespace Orion.Integration.Kafka.NotificationHandlers;

public sealed class FeedNotificationHandler:
    INotificationHandler<DomainEventNotification<FeedCreationDomainEvent>>
{
    public Task Handle(DomainEventNotification<FeedCreationDomainEvent> notification, CancellationToken cancellationToken)
    {
        var domainEvents = notification.DomainEvents;
        
        if (!domainEvents.Any())
        {
            return Task.CompletedTask;
        }

        var pool = domainEvents.Select(v => PublishIntegrationEvent(v, u=>u.ToIntegrationEvent(), cancellationToken));

        return Task.WhenAll(pool);
    }
    
    private Task PublishIntegrationEvent<TDomainEvent>(
        TDomainEvent domainEvent,
        Func<TDomainEvent, FeedCreationIntegrationEvent> converter,
        CancellationToken cancellationToken) where TDomainEvent: FeedBaseEvent
    {
        var key = $"DataProviderId_{domainEvent.FeedId}";
        var eventPlayload = converter(domainEvent);

        throw new NotImplementedException("Add Kafka publisher");
    }
}