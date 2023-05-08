using Contracts.Repositories;
using Contracts.Security;
using Domain.Models;
using MediatR;
using NetTopologySuite.Geometries;

namespace Application.Commands.Kitchens;

public record EditKitchenShippingAreasCommand(Guid KitchenId, List<Polygon> Areas) : IRequest;

internal sealed class EditKitchenShippingAreasHandler : IRequestHandler<EditKitchenShippingAreasCommand>
{
    private readonly IRepositoryManager _repository;

    public async Task Handle(EditKitchenShippingAreasCommand request, CancellationToken cancellationToken)
    {
        var kitchen = await _repository.Kitchens
            .GetKitchenAsync(request.KitchenId);

        if (kitchen.Settings is null)
            kitchen.ChangeSettings(new KitchenSettings());
        
        kitchen.Settings!.EditShippingAreas(request.Areas);

        _repository.Kitchens.UpdateKitchen(kitchen);
        await _repository.SaveAsync(cancellationToken);
    }

    public EditKitchenShippingAreasHandler(IRepositoryManager repository, ICurrentUserService currentUserService)
    {
        _repository = repository;
    }
}