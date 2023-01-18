namespace Domain.Models;

public class GroupKitchenSettings
{
    public Guid Id { get; private set; }
    public Guid GroupId { get; private set; }
    public string TargetEmail { get; set; }
    public string KitchenName { get; private set; }
    public int HourExpired { get; private set; }
    public int MinuteExpired { get; private set; }
    public PeriodicRefresh PeriodicRefresh { get; private set; }
    public MenuFormat MenuFormat { get; private set; }

    private GroupKitchenSettings(){ }
    
    public GroupKitchenSettings(
        Guid groupId,
        string kitchenName, 
        int hourExpired, 
        int minuteExpired, 
        PeriodicRefresh periodicRefresh, 
        MenuFormat menuFormat,
        string targetEmail)
    {
        Id = Guid.NewGuid();
        GroupId = groupId;
        KitchenName = kitchenName;
        HourExpired = hourExpired;
        MinuteExpired = minuteExpired;
        PeriodicRefresh = periodicRefresh;
        MenuFormat = menuFormat;
        TargetEmail = targetEmail;
    }
}

public enum PeriodicRefresh
{
    Daily,
    Weekly
}

public enum MenuFormat
{
    Text,
    Excel
}