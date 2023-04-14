using Contracts.Repositories;
using Domain.Exceptions;
using MediatR;
using Shared.DataTransferObjects.Group;
using Shared.DataTransferObjects.Kitchen;

namespace Application.Queries.Kitchen;

public sealed record GetKitchenQuery(Guid Id) : IRequest<KitchenDto>;

internal class GetGroupHandler : IRequestHandler<GetKitchenQuery, KitchenDto>
{
    private readonly IRepositoryManager _repository;

    public async Task<KitchenDto> Handle(GetKitchenQuery request, CancellationToken cancellationToken)
    {
        var group = await _repository.Kitchens.GetKitchenAsync(request.Id, false);

        return group.Map();
    }

    public GetGroupHandler(IRepositoryManager repository)
    {
        _repository = repository;
    }
}