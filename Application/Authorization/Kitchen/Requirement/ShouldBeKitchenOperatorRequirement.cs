using Contracts.Repositories;
using MediatR.Behaviors.Authorization;

namespace Application.Authorization.Kitchen.Requirement;

public class ShouldBeKitchenOperatorRequirement : IAuthorizationRequirement
{
    public Guid UserId { get; set; } 
    public Guid KitchenId { get; set; }

    private class ShouldBeKitchenOperatorHandler : IAuthorizationHandler<ShouldBeKitchenOperatorRequirement>
    {
        private readonly IRepositoryManager _repository; 
        
        public async Task<AuthorizationResult> Handle(ShouldBeKitchenOperatorRequirement request, CancellationToken cancellationToken)
        {
            var userId = request.UserId;
            var kitchen = await _repository.Kitchens.GetKitchenAsync(request.KitchenId);

            if (kitchen.Managers.All(x => x.Id != userId))
                return AuthorizationResult.Fail();

            return AuthorizationResult.Succeed();
        }

        public ShouldBeKitchenOperatorHandler(IRepositoryManager repository)
        {
            _repository = repository;
        }
    }
}