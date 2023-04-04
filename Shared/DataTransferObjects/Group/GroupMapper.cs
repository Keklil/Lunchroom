using Domain.Models;

namespace Shared.DataTransferObjects.Group;

public static class GroupMapper
{
    public static GroupDto Map(this Domain.Models.Group source)
    {
        return new GroupDto(
            source.Id,
            source.Admin.Id,
            source.OrganizationName,
            source.Address,
            source.Members.Select(sourceMember => sourceMember.Id),
            source.Referral,
            source.Settings != null
                ? new KitchenSettingsDto(
                    source.Settings.TargetEmail,
                    source.Settings.KitchenName,
                    source.Settings.HourExpired,
                    source.Settings.MinuteExpired,
                    source.Settings.PeriodicRefresh,
                    source.Settings.MenuFormat)
                : null,
            source.PaymentInfo != null
                ? new PaymentInfoDto
                {
                    GroupId = source.PaymentInfo.GroupId,
                    Link = source.PaymentInfo.Link,
                    Description = source.PaymentInfo.Description,
                    Qr = source.PaymentInfo.Qr
                }
                : null);
    }

    public static GroupKitchenSettings Map(this GroupConfigDto source)
    {
        return new GroupKitchenSettings(
            source.GroupId,
            source.KitchenName,
            source.HourExpired,
            source.MinuteExpired,
            source.Refresh,
            source.MenuFormat,
            source.TargetEmail);
    }

    public static PaymentInfo Map(this PaymentInfoDto source)
    {
        return new PaymentInfo
        {
            GroupId = source.GroupId,
            Link = source.Link,
            Description = source.Description,
            Qr = source.Qr
        };
    }
}