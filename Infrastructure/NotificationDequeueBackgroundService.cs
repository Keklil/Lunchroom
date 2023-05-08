using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace Infrastructure;

public class NotificationDequeueBackgroundService : BackgroundService
{
    private readonly ILogger<NotificationDequeueBackgroundService> _logger;
    private readonly IServiceProvider _serviceProvider;

    public NotificationDequeueBackgroundService(BackgroundNotificationQueue notificationQueue, 
        ILogger<NotificationDequeueBackgroundService> logger, IServiceProvider serviceProvider)
    {
        NotificationQueue = notificationQueue;
        _logger = logger;
        _serviceProvider = serviceProvider;
    }

    private BackgroundNotificationQueue NotificationQueue { get; }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        _logger.LogInformation("Сервис фоновой очереди задач запущен.");

        await BackgroundProcessing(stoppingToken);
    }

    private async Task BackgroundProcessing(CancellationToken stoppingToken)
    {
        using var scope = _serviceProvider.CreateScope();
        var publisher = scope.ServiceProvider.GetRequiredService<IPublisher>();
        
        while (!stoppingToken.IsCancellationRequested)
        {
            var domainNotification = await NotificationQueue.DequeueAsync();
            
            await publisher.Publish(domainNotification, stoppingToken);
        }
    }

    public override async Task StopAsync(CancellationToken stoppingToken)
    {
        _logger.LogInformation("Сервис фоновой очереди задач остановлен.");

        await base.StopAsync(stoppingToken);
    }
}