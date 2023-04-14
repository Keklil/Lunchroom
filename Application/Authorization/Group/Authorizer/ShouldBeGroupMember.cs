using Application.Authorization.Group.Requirement;
using Application.Queries.Groups;
using Contracts.Security;
using MediatR.Behaviors.Authorization;

namespace Application.Authorization.Group.Authorizer;

public class ShouldBeGroupMember : AbstractRequestAuthorizer<GetGroupQuery>
{
    private readonly ICurrentUserService _currentUserService;

    public override void BuildPolicy(GetGroupQuery request)
    {
        UseRequirement(new ShouldBeGroupMemberRequirement()
        {
            UserId = _currentUserService.GetUserId(),
            GroupId = request.Id
        });
    }

    public ShouldBeGroupMember(ICurrentUserService currentUserService)
    {
        _currentUserService = currentUserService;
    }
}