using Domain.Notifications;

namespace Domain.Models.Base;

public abstract class Entity
{
    public Guid Id { get; protected init; } = Guid.NewGuid();
    
    private List<DomainNotification>? _domainEvents;
    public IReadOnlyCollection<DomainNotification> DomainEvents => _domainEvents;

    public void AddDomainEvent(DomainNotification eventItem)
    {
        _domainEvents = _domainEvents ?? new List<DomainNotification>();
        _domainEvents.Add(eventItem);
    }

    public void RemoveDomainEvent(DomainNotification eventItem)
    {
        _domainEvents?.Remove(eventItem);
    }
    
    public void ClearDomainEvents()
    {
        _domainEvents?.Clear();
    }
}