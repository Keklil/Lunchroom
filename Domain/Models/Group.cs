using Domain.Exceptions;
using Domain.Exceptions.AuthExceptions;
using Domain.Infrastructure;
using Domain.Models.Base;
using NetTopologySuite.Geometries;

namespace Domain.Models;

public class Group : Entity
{
    public User Admin { get; private set; }
    public string OrganizationName { get; private set; }
    public IReadOnlyCollection<User> Members => _members;
    private readonly List<User> _members = new();
    public GroupReferral Referral { get; private set; }
    public GroupSettings? Settings { get; private set; }
    public Guid? SelectedKitchenId { get; private set; }
    public PaymentInfo? PaymentInfo { get; set; }

    public void AddMember(User member)
    {
        _members.Add(member);
    }

    public void SetReferralToken(string referToken)
    {
        if (string.IsNullOrWhiteSpace(referToken))
            throw new AttemptSetNullOrEmptyToken();
        
        Referral = new(referToken);
    }

    public void SetSettings(GroupSettings settings)
    {
        Settings = settings;
    }

    public void SetPaymentInfo(PaymentInfo paymentInfo)
    {
        PaymentInfo = paymentInfo;
    }

    public void SelectKitchen(Kitchen kitchen)
    {
        SelectedKitchenId = kitchen.Id;
    }
    
    public Group(User admin, string organizationName)
    {
        Id = Guid.NewGuid();
        OrganizationName = organizationName;
        Admin = admin;
        _members.Add(admin);
    }
    
    private Group() { }
}