using Contracts;
using Contracts.Repositories;
using Domain.Notifications;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Application.Notifications;

internal class MenuPublishedHandler : INotificationHandler<MenuPublished>
{
    private readonly IRepositoryManager _repository;
    private readonly IPushSender _pushSender;
    private readonly ILogger<MenuPublishedHandler> _logger; 

    public async Task Handle(MenuPublished notification, CancellationToken cancellationToken)
    {
        try
        {
            var kitchen = await _repository.Kitchens.GetKitchenAsync(notification.KitchenId);
            var usersDevicesInfo = await _repository.User.GetUsersDevicesInfoBySelectedKitchenInGroups(notification.KitchenId);

            await _pushSender.Push(
                "Меню уже доступно!",
                $"Функция заказа будет доступна до {kitchen.Settings?.LimitingTimeForOrder}",
                usersDevicesInfo.Select(x => x.DeviceToken).ToList());
        }
        catch (Exception e)
        {
            _logger.LogError(e,"Ошибка во время обработки собития публикации меню {MenuId}", notification.MenuId);
        }
    }

    public MenuPublishedHandler(IRepositoryManager repository, IPushSender pushSender, ILogger<MenuPublishedHandler> logger)
    {
        _repository = repository;
        _pushSender = pushSender;
        _logger = logger;
    }
}