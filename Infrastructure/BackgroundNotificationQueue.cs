using System.Threading.Channels;
using MediatR;

namespace Infrastructure;

public class BackgroundNotificationQueue
{
    private readonly Channel<INotification> _queue;

    public BackgroundNotificationQueue()
    {
        var options = new BoundedChannelOptions(1000)
        {
            FullMode = BoundedChannelFullMode.Wait
        };
        _queue = Channel.CreateBounded<INotification>(options);
    }

    public async Task EnqueueAsync(INotification domainNotification)
    {
        if (domainNotification == null)
        {
            throw new ArgumentNullException(nameof(domainNotification));
        }

        await _queue.Writer.WriteAsync(domainNotification);
    }

    public async Task<INotification> DequeueAsync()
    {
        var domainNotification = await _queue.Reader.ReadAsync();

        return domainNotification;
    }
}