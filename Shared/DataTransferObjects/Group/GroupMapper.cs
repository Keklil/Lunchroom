using Domain.Models;
using Shared.DataTransferObjects.User;

namespace Shared.DataTransferObjects.Group;

public static class GroupMapper
{
    public static GroupDto Map(this Domain.Models.Group source)
    {
        return new GroupDto(
            source.Id,
            source.Admin.Map(),
            source.OrganizationName,
            source.Members.Select(sourceMember => sourceMember.Id),
            source.Referral,
            source.Settings != null
                ? new GroupKitchenSettingsDto(
                    source.Settings.Address,
                    source.Settings.Location)
                : null,
            source.PaymentInfo != null
                ? new PaymentInfoDto
                {
                    Link = source.PaymentInfo.Link,
                    Description = source.PaymentInfo.Description,
                    Qr = source.PaymentInfo.Qr
                }
                : null,
            source.SelectedKitchenId);
    }

    public static GroupSettings Map(this GroupConfigDto source)
    {
        return new GroupSettings(
            source.GroupId,
            source.Address,
            source.Location);
    }

    public static PaymentInfo Map(this PaymentInfoDto source)
    {
        return new PaymentInfo (source.Link, source.Description, source.Qr);
    }
}