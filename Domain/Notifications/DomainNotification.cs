using MediatR;

namespace Domain.Notifications;

public abstract class DomainNotification : INotification, ICloneable
{
    public object Clone()
    {
        return MemberwiseClone();
    }
}