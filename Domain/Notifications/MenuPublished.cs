namespace Domain.Notifications;

public class MenuPublished : DomainNotification
{
    public Guid MenuId { get; }
    public Guid KitchenId { get; }

    public MenuPublished(Guid menuId, Guid kitchenId)
    {
        MenuId = menuId;
        KitchenId = kitchenId;
    }
}