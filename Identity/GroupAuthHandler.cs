using Contracts.Repositories;
using Domain.Models;
using Microsoft.AspNetCore.Authorization;

namespace Identity;

public class GroupAuthHandler : AuthorizationHandler<IsInGroupRequirement, Group>
{
    protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, IsInGroupRequirement requirement, Group group)
    {
        if (group.Id == requirement.GroupId)
        {
            context.Succeed(requirement);
        }
        
        return Task.CompletedTask;
    }
}

public class IsInGroupRequirement : IAuthorizationRequirement
{
    public Guid GroupId { get; }
    public IsInGroupRequirement(Guid groupId)
    {
        GroupId = groupId;
    }
}