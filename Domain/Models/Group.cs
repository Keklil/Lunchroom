﻿using Domain.Exceptions;
using Domain.Infrastructure;

namespace Domain.Models;

public class Group
{
    public Guid Id { get; private set; }
    public User Admin { get; private set; }
    public string OrganizationName { get; private set; }
    public string Address { get; private set; }
    public IReadOnlyCollection<User> Members => _members;
    private List<User> _members = new();
    public GroupReferral Referral { get; }

    private Group()
    {
        
    }
    
    public Group(User admin, string organizationName, string address)
    {
        if (admin.Role is not Role.Admin)
            throw new AttemptCreateGroupByNonAdmin();
        
        Id = Guid.NewGuid();
        OrganizationName = organizationName;
        Address = address;
        Admin = admin;

        Referral = new GroupReferral(Id);
    }

    public void AddMember(User member)
    {
        _members.Add(member);
    }

    public void SetReferralToken(string referToken)
    {
        if (string.IsNullOrWhiteSpace(referToken))
            throw new AttemptSetNullOrEmptyToken();
        Referral.SetReferToken(referToken);
    }
}