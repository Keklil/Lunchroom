using Domain.Models;
using Shared.DataTransferObjects.Group;

namespace Shared.DataTransferObjects.Kitchen;

public static class KitchenMapper
{
    public static KitchenDto Map(this Domain.Models.Kitchen kitchen)
    {
        return new KitchenDto(
            Id: kitchen.Id,
            OrganizationName: kitchen.OrganizationName, 
            Address: kitchen.Address, 
            Inn: kitchen.Inn, 
            Contacts: kitchen.Contacts, 
            AllowForOrders: kitchen.AllowForOrders, 
            Managers: kitchen.Managers
                .Select(kitchenManager => new User.UserDto
                {
                    Id = kitchenManager.Id,
                    Surname = kitchenManager.Surname,
                    Name = kitchenManager.Name,
                    Patronymic = kitchenManager.Patronymic,
                    Email = kitchenManager.Email,
                    Groups = kitchenManager.Groups.Select(kitchenManagerGroup => kitchenManagerGroup.Id).ToList()
                }).ToList(), 
            Settings: kitchen.Settings?.Map());
    }

    private static KitchenSettingsDto Map(this Domain.Models.KitchenSettings settings)
    {
        return new KitchenSettingsDto(
            LimitingTimeForOrder: settings.LimitingTimeForOrder, 
            MenuUpdatePeriod: settings.MenuUpdatePeriod, 
            MenuFormat: settings.MenuFormat,
            MinAmountForSharedOrder: settings.MinAmountForSharedOrder,
            ShippingAreas: settings.ShippingAreas.ToList());
    }

    public static KitchenSettings Map(this KitchenSettingsForEditDto source, KitchenSettings? kitchenSettings)
    {
        if (kitchenSettings == null)
        {
            return new KitchenSettings(
                limitingTimeForOrder: source.LimitingTimeForOrder, 
                menuUpdatePeriod: source.MenuUpdatePeriod, 
                menuFormat: source.MenuFormat,
                source.MinAmountForSharedOrder);
        }
        else
        {
            kitchenSettings.ChangeMenuFormat(source.MenuFormat);
            kitchenSettings.ChangeMenuUpdatePeriod(source.MenuUpdatePeriod);
            kitchenSettings.EditLimitingTimeForOrder(source.LimitingTimeForOrder);
            return kitchenSettings;
        }
    }

    public static AvailableKitchensDto MapToAvailableKitchensDto(this Domain.Models.Kitchen kitchen)
    {
        return new AvailableKitchensDto
        {
            KitchenId = kitchen.Id,
            OrganizationName = kitchen.OrganizationName,
            Address = kitchen.Address,
            Contacts = kitchen.Contacts,
            Settings = kitchen.Settings?.Map()
        };
    }
}