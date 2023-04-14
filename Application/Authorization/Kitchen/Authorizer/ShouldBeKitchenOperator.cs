using Application.Authorization.Group.Requirement;
using Application.Authorization.Kitchen.Requirement;
using Application.Commands.Kitchens;
using Application.Interfaces;
using Contracts.Security;
using MediatR.Behaviors.Authorization;

namespace Application.Authorization.Kitchen.Authorizer;

public class ShouldBeKitchenOperator<TRequest> : AbstractRequestAuthorizer<TRequest>
    where TRequest : IKitchenRequest
{
    private readonly ICurrentUserService _currentUserService;

    public ShouldBeKitchenOperator(ICurrentUserService currentUserService)
    {
        _currentUserService = currentUserService;
    }

    public override void BuildPolicy(TRequest request)
    {
        UseRequirement(new ShouldBeKitchenOperatorRequirement()
             {
                 UserId = _currentUserService.GetUserId(),
                 GroupId = request.KitchenId
             });
    }
}