using Contracts.Repositories;
using Contracts.Security;
using Domain.Infrastructure;
using MediatR;

namespace Application.Commands.Users;

public record SubscribeToNotificationsCommand(string DeviceToken) : IRequest;

internal class SubscribeToNotificationsHandler : IRequestHandler<SubscribeToNotificationsCommand>
{
    private readonly IRepositoryManager _repository;
    private readonly ICurrentUserService _currentUserService;

    public async Task Handle(SubscribeToNotificationsCommand request, CancellationToken cancellationToken)
    {
        var user = await _repository.User.GetUserAsync(_currentUserService.GetUserId());

        var deviceInfo = await _repository.User.GetUserDeviceInfoAsync(user.Id);

        if (deviceInfo is null)
        {
            deviceInfo = new UserDeviceInfo(request.DeviceToken, user.Id);
            _repository.User.AddUserDeviceInfo(deviceInfo);
        }

        await _repository.SaveAsync(cancellationToken);
    }

    public SubscribeToNotificationsHandler(IRepositoryManager repository, ICurrentUserService currentUserService)
    {
        _repository = repository;
        _currentUserService = currentUserService;
    }
}
