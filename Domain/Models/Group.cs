using Domain.Exceptions;

namespace Domain.Models;

public class Group
{
    public Guid Id { get; private set; }
    public User Admin { get; private set; }
    public IReadOnlyCollection<User> Members => _members;
    private List<User> _members = new();
    public string ReferLink { get; private set; }
    public string ReferCode { get; private set; }

    public Group(User admin)
    {
        if (admin.Role is not Role.Admin)
            throw new AttemptCreateGroupByNonAdmin();
        
        Id = new Guid();
        Admin = admin;
    }

    public void AddMember(User member)
    {
        _members.Add(member);
    }
}