using Domain.Exceptions;

namespace Domain.Infrastructure;

public class UserDeviceInfo
{
    public Guid Id { get; private set; }
    public Guid UserId { get; private set; }
    public string DeviceToken { get; private set;  }

    public void UpdateToken(string deviceToken)
    {
        if (string.IsNullOrEmpty(deviceToken))
            throw new DomainException("Пустой токен");
        
        DeviceToken = deviceToken;
    }
    
    public UserDeviceInfo(string deviceToken, Guid userId)
    {
        Id = Guid.NewGuid();
        DeviceToken = deviceToken;
        UserId = userId;
    }
    
    private UserDeviceInfo() { }
}