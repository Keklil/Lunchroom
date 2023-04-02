using Contracts;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Application.Notifications;

public sealed record TodayOrderConfirmedNotification : INotification;

internal sealed class TodayOrderConfirmedHandler : INotificationHandler<TodayOrderConfirmedNotification>
{
    private readonly ILogger<TodayOrderConfirmedHandler> _logger;
    private readonly IMailParser _mailParser;
    private readonly ISender _sender;

    public async Task Handle(TodayOrderConfirmedNotification notification, CancellationToken cancellationToken)
    {
    }

    public TodayOrderConfirmedHandler(ILogger<TodayOrderConfirmedHandler> logger, ISender sender, IMailParser mailService)
    {
        _logger = logger;
        _sender = sender;
        _mailParser = mailService;
    }
}