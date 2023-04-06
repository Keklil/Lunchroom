using Contracts.Repositories;
using MediatR.Behaviors.Authorization;

namespace Application.Authorization.Group.Requirement;

public class ShouldBeGroupMemberRequirement : IAuthorizationRequirement
{
    public Guid UserId { get; set; } 
    public Guid GroupId { get; set; }

    class GroupAuthHandler : IAuthorizationHandler<ShouldBeGroupMemberRequirement>
    {
        private readonly IRepositoryManager _repository; 
        
        public async Task<AuthorizationResult> Handle(ShouldBeGroupMemberRequirement request, CancellationToken cancellationToken)
        {
            var userId = request.UserId;
            var userGroups = await _repository.User.GetUserGroupIdsAsync(userId);

            if (!userGroups.Contains(request.GroupId))
                return AuthorizationResult.Fail();

            return AuthorizationResult.Succeed();
        }

        public GroupAuthHandler(IRepositoryManager repository)
        {
            _repository = repository;
        } 
    }
}