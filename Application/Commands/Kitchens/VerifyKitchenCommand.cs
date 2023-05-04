
using Contracts.Repositories;
using Contracts.Security;
using Domain.Infrastructure;
using MediatR;

namespace Application.Commands.Kitchens;

public sealed record VerifyKitchenCommand(Guid KitchenId) : IRequest;

internal sealed class VerifyKitchenHandler : IRequestHandler<VerifyKitchenCommand>
{
    private readonly IRepositoryManager _repository;
    private readonly ICurrentUserService _currentUserService;

    public async Task Handle(VerifyKitchenCommand request, CancellationToken cancellationToken)
    {
        var kitchen = await _repository.Kitchens.GetKitchenAsync(request.KitchenId);
        
        var checker = await _repository.User.GetUserAsync(_currentUserService.GetUserId());
        
        kitchen.VerifyKitchen();
        await _repository.Kitchens.SaveVerifyStamp(new KitchenVerificationStamp(checker));

        _repository.Kitchens.UpdateKitchen(kitchen);
        await _repository.SaveAsync(cancellationToken);
    }

    public VerifyKitchenHandler(IRepositoryManager repository, ICurrentUserService currentUserService)
    {
        _repository = repository;
        _currentUserService = currentUserService;
    }
}