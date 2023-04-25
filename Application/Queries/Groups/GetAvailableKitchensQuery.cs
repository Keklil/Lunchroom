using Contracts.Repositories;
using MediatR;
using Shared.DataTransferObjects.Group;
using Shared.DataTransferObjects.Kitchen;

namespace Application.Queries.Groups;

public sealed record GetAvailableKitchensQuery(Guid GroupId) : IRequest<List<AvailableKitchensDto>>;

internal class GetAllowedKitchenQueryHandler : IRequestHandler<GetAvailableKitchensQuery, List<AvailableKitchensDto>>
{
    private readonly IRepositoryManager _repository;
    
    public async Task<List<AvailableKitchensDto>> Handle(GetAvailableKitchensQuery request, CancellationToken cancellationToken)
    {
        var groupLocation = (await _repository.Groups.GetGroupAsync(request.GroupId)).Settings.Location;
        
        var kitchens = await _repository.Kitchens.GetKitchensByLocationAsync(groupLocation);
        
        return kitchens.Select(kitchen => kitchen.MapToAvailableKitchensDto()).ToList();
    }

    public GetAllowedKitchenQueryHandler(IRepositoryManager repository)
    {
        _repository = repository;
    }
}