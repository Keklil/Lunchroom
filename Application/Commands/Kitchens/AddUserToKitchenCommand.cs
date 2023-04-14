using Application.Interfaces;
using Contracts.Repositories;
using Domain.Exceptions.KitchenExceptions;
using MediatR;

namespace Application.Commands.Kitchens;

public sealed record AddUserToKitchenCommand(Guid UserId, Guid KitchenId) : IRequest, IKitchenRequest;

internal sealed class AddUserToKitchenHandler : IRequestHandler<AddUserToKitchenCommand>
{
    private readonly IRepositoryManager _repository;

    public async Task Handle(AddUserToKitchenCommand request, CancellationToken cancellationToken)
    {
        var kitchen = await _repository.Kitchens.GetKitchenAsync(request.KitchenId);

        var user = await _repository.User.GetUserAsync(request.UserId);

        if (kitchen.Managers.Any(x => x.Id == user.Id))
            throw new UserAlreadyInKitchenException();

        kitchen.AddManager(user);

        _repository.Kitchens.UpdateKitchen(kitchen);
        await _repository.SaveAsync(cancellationToken);
    }

    public AddUserToKitchenHandler(IRepositoryManager repository)
    {
        _repository = repository;
    }
}