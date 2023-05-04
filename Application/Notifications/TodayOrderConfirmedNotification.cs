using Contracts;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Application.Notifications;

public sealed record TodayOrderConfirmedNotification : INotification;

internal sealed class TodayOrderConfirmedHandler : INotificationHandler<TodayOrderConfirmedNotification>
{
    private readonly ILogger<TodayOrderConfirmedHandler> _logger;
    private readonly IPlainTextParser _plainTextParser;
    private readonly ISender _sender;

    public async Task Handle(TodayOrderConfirmedNotification notification, CancellationToken cancellationToken)
    {
    }

    public TodayOrderConfirmedHandler(ILogger<TodayOrderConfirmedHandler> logger, ISender sender, IPlainTextParser service)
    {
        _logger = logger;
        _sender = sender;
        _plainTextParser = service;
    }
}