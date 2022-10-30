using Contracts;
using MediatR;
using System.Text.Encodings.Web;
using System.Text.Json;

namespace Application.Notifications;

public sealed record TodayOrderConfirmedNotification() : INotification;

internal sealed class TodayOrderConfirmedHandler : INotificationHandler<TodayOrderConfirmedNotification>
{
    private readonly ILoggerManager _logger;
    private readonly ISender _sender;
    private readonly IMailParser _mailParser;
    
    public TodayOrderConfirmedHandler(ILoggerManager logger, ISender sender, IMailParser mailService)
    {
        _logger = logger;
        _sender = sender;
        _mailParser = mailService;
    }

    public async Task Handle(TodayOrderConfirmedNotification notification, CancellationToken cancellationToken)
    {
        
    }
}
