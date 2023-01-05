using Domain.Exceptions;

namespace Domain.Infrastructure;

public class GroupReferral
{
    public string ReferToken { get; private set; }

    public GroupReferral()
    {

    }

    public void SetReferToken(string referToken)
    {
        ReferToken = referToken;
    }
}