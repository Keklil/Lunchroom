using Domain.Models.Enums;

namespace Domain.Models;

public class GroupKitchenSettings
{
    public Guid Id { get; }
    public Guid GroupId { get; }
    public string TargetEmail { get; set; }
    
    private GroupKitchenSettings(){ }
    
    public GroupKitchenSettings(
        Guid groupId,
        string targetEmail)
    {
        Id = Guid.NewGuid();
        GroupId = groupId;
        TargetEmail = targetEmail;
    }
}

