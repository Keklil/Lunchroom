using Domain.DataTransferObjects.User;
using Domain.Models;
using Microsoft.AspNetCore.Http;

namespace Domain.DataTransferObjects.Group;

public static class GroupMapper
{
    public static GroupDto Map(this Models.Group source)
    {
        return new GroupDto(
            Id: source.Id, 
            AdminId: source.Admin.Id, 
            OrganizationName: source.OrganizationName, 
            Address: source.Address, 
            Members: source.Members.Select(sourceMember => sourceMember.Id), 
            Referral: source.Referral, 
            Settings: source.Settings != null 
                ? new KitchenSettingsDto(
                    TargetEmail: source.Settings.TargetEmail,
                    KitchenName: source.Settings.KitchenName, 
                    HourExpired: source.Settings.HourExpired, 
                    MinuteExpired: source.Settings.MinuteExpired, 
                    PeriodicRefresh: source.Settings.PeriodicRefresh, 
                    MenuFormat: source.Settings.MenuFormat)
                : null, 
            PaymentInfo: source.PaymentInfo != null 
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
            groupId: source.GroupId, 
            kitchenName: source.KitchenName, 
            hourExpired: source.HourExpired, 
            minuteExpired: source.MinuteExpired, 
            periodicRefresh: source.Refresh, 
            menuFormat: source.MenuFormat,
            targetEmail: source.TargetEmail);
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