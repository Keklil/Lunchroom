using Domain.Models.Base;
using MediatR;

namespace Infrastructure.Extensions;

public static class MediatorExtensions
{
    public static async Task DispatchDomainEvents(this IPublisher mediator, Entity entity) 
    {
        foreach (var domainEvent in entity.DomainEvents)
            await mediator.Publish(domainEvent);
        
        entity.ClearDomainEvents();
    }
}
