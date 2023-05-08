using Domain.Models.Base;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Infrastructure.Extensions;

public class EventsDispatcher
{
    private readonly ILogger<EventsDispatcher> _logger;
    private readonly BackgroundNotificationQueue _notificationQueue;

    public async Task DispatchDomainEvents(Entity entity)
    {
        try
        {
            foreach (var domainEvent in entity.DomainEvents)
            {
                var eventCopy = domainEvent.Clone();
                await _notificationQueue.EnqueueAsync(eventCopy as INotification);
            }

            entity.ClearDomainEvents();
        }
        catch (Exception e)
        {
            _logger.LogError(e, "Возникла ошибка во время обработки событий сущности {Type} {EntityId}, ", entity.GetType(), entity.Id);
            throw;
        }
    }

    public EventsDispatcher(ILogger<EventsDispatcher> logger, BackgroundNotificationQueue notificationQueue)
    {
        _logger = logger;
        _notificationQueue = notificationQueue;
    }
}
