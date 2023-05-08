using Contracts.Repositories;
using Domain.Exceptions;
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
        var group = await _repository.Groups.GetGroupAsync(request.GroupId);
        
        if (group.Settings is null)
            throw new DomainException("У группы с id {GroupId} не указанны данные о локации.", group.Id);
        
        var kitchens = await _repository.Kitchens.GetKitchensByLocationAsync(group.Settings.Location);
        
        return kitchens.Select(kitchen => kitchen.MapToAvailableKitchensDto()).ToList();
    }

    public GetAllowedKitchenQueryHandler(IRepositoryManager repository)
    {
        _repository = repository;
    }
}