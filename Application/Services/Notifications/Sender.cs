using Contracts;
using FirebaseAdmin.Messaging;

namespace Application.Services.Notifications;

public class Sender : IPushSender
{
    public Task Push(string title, string message, IReadOnlyList<string> deviceTokens, IReadOnlyDictionary<string, string>? additionalData = null)
    {
        var notifications = new MulticastMessage()
        {
            Tokens = deviceTokens,
            Notification = new Notification()
            {
                Title = title,
                Body = message
            },
            Data = additionalData
            
        };
        
        FirebaseMessaging.DefaultInstance.SendMulticastAsync(notifications);
        
        return Task.CompletedTask;
    }
}