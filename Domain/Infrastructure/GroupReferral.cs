using Domain.Exceptions;

namespace Domain.Infrastructure;

public class GroupReferral
{
    public Guid Id { get; private set; }
    public Guid GroupId { get; set; }
    public string ReferToken { get; private set; }

    public GroupReferral(Guid groupId)
    {
        Id = new Guid();
        GroupId = groupId;
    }

    public void SetReferToken(string referToken)
    {
        ReferToken = referToken;
    }
}