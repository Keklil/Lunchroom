using Contracts.Repositories;
using Contracts.Security;
using MediatR;
using NetTopologySuite.Geometries;
using Shared.DataTransferObjects.Kitchen;

namespace Application.Commands.Kitchens;

public record EditKitchenShippingAreasCommand(Guid KitchenId, List<Polygon> Settings) : IRequest;

internal sealed class EditKitchenShippingAreasHandler : IRequestHandler<EditKitchenShippingAreasCommand>
{
    private readonly IRepositoryManager _repository;
    private readonly ICurrentUserService _currentUserService;

    public async Task Handle(EditKitchenShippingAreasCommand request, CancellationToken cancellationToken)
    {
        var kitchen = await _repository.Kitchens
            .GetKitchenAsync(request.KitchenId);
        
        var kitchenSettings = await _repository.Kitchens
            .GetKitchenSettingsAsync(request.KitchenId);
        
        kitchenSettings.EditShippingAreas(request.Settings);
        kitchen.EditSettings(kitchenSettings);
        
        _repository.Kitchens.UpdateKitchen(kitchen);
        await _repository.SaveAsync(cancellationToken);
    }

    public EditKitchenShippingAreasHandler(IRepositoryManager repository, ICurrentUserService currentUserService)
    {
        _repository = repository;
        _currentUserService = currentUserService;
    }
}