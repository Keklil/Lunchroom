using Domain.Exceptions;

namespace Domain.Infrastructure;

public class GroupReferral
{    
    public string ReferToken { get; private set; }
    
    public GroupReferral(string referToken)
    {
        ReferToken = referToken;
    }
}