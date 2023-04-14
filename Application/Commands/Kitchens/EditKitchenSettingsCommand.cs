using Application.Interfaces;
using Contracts.Repositories;
using Contracts.Security;
using MediatR;
using Shared.DataTransferObjects.Kitchen;

namespace Application.Commands.Kitchens;

public record EditKitchenSettingsCommand(Guid KitchenId, KitchenSettingsForEditDto Settings) : IRequest, IKitchenRequest;

internal sealed class EditKitchenSettingsHandler : IRequestHandler<EditKitchenSettingsCommand>
{
    private readonly IRepositoryManager _repository;
    private readonly ICurrentUserService _currentUserService;

    public async Task Handle(EditKitchenSettingsCommand request, CancellationToken cancellationToken)
    {
        var kitchen = await _repository.Kitchens
            .GetKitchenAsync(request.KitchenId);
        
        var kitchenSettings = await _repository.Kitchens
            .GetKitchenSettingsAsync(request.KitchenId);
        
        kitchen.EditSettings(request.Settings.Map(request.KitchenId, kitchenSettings));
        
        _repository.Kitchens.UpdateKitchen(kitchen);
        await _repository.SaveAsync(cancellationToken);
    }

    public EditKitchenSettingsHandler(IRepositoryManager repository, ICurrentUserService currentUserService)
    {
        _repository = repository;
        _currentUserService = currentUserService;
    }
}