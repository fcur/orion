using System.IO.Pipes;
using MediatR;
using Orion.App.Domain.SiriusEntity.DomainEvents;
using Orion.App.Domain.SiriusEntity.Events;
using Orion.Integration.Kafka.IntegrationEvents;
using Venus.Shared.Domain;
using FeedCreationIntegrationEvent = Orion.Integration.Kafka.IntegrationEvents.FeedCreationEvent;

namespace Orion.Integration.Kafka.NotificationHandlers;

public sealed class FeedNotificationHandler:
    INotificationHandler<DomainEventNotification<SiriusContentCreationEvent>>
{
    public Task Handle(DomainEventNotification<SiriusContentCreationEvent> notification, CancellationToken cancellationToken)
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
        CancellationToken cancellationToken) where TDomainEvent: SiriusContentBaseEvent
    {
        var key = $"DataProviderId_{domainEvent.FeatureId}";
        var eventPlayload = converter(domainEvent);

        throw new NotImplementedException("Add Kafka publisher");
    }
}