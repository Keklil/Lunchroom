using NetTopologySuite.Geometries;

namespace Domain.Models;

public class GroupSettings
{
    public Guid Id { get; }
    public Guid GroupId { get; }
    public string Address { get; private set; }
    public Point Location { get; private set; }
    
    private GroupSettings(){ }
    
    public GroupSettings(
        Guid groupId,
        string address,
        Point location)
    {
        Id = Guid.NewGuid();
        GroupId = groupId;
        Address = address;
        Location = location;
    }
}

