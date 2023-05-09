
using Contracts.Repositories;
using Contracts.Security;
using Domain.Exceptions.AuthExceptions;
using Domain.Models;
using Domain.Models.Enums;
using MediatR;
using Shared.DataTransferObjects.Kitchen;

namespace Application.Commands.Kitchens;

public sealed record CreateKitchenCommand(KitchenForCreationDto Kitchen) : IRequest<KitchenDto>;

internal sealed class CreateKitchenHandler : IRequestHandler<CreateKitchenCommand, KitchenDto>
{
    private readonly IRepositoryManager _repository;
    private readonly ICurrentUserService _currentUserService;

    public async Task<KitchenDto> Handle(CreateKitchenCommand request, CancellationToken cancellationToken)
    {
        var currentUserId = _currentUserService.GetUserId();
        var user = await _repository.User.GetUserAsync(currentUserId);
        
        if (user.Role != Role.KitchenOperator)
            throw new AttemptCreateKitchenByNonKitchenOperator();

        var kitchen = new Kitchen(request.Kitchen.OrganizationName, request.Kitchen.Address, request.Kitchen.Inn, request.Kitchen.Contacts.Email, request.Kitchen.Contacts.Phone);
        kitchen.AddManager(user);
        
        _repository.Kitchens.CreateKitchen(kitchen);
        await _repository.SaveAsync(cancellationToken);

        return kitchen.Map();
    }

    public CreateKitchenHandler(IRepositoryManager repository, ICurrentUserService currentUserService)
    {
        _repository = repository;
        _currentUserService = currentUserService;
    }
}