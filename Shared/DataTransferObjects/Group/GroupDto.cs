using Domain.Infrastructure;
using Shared.DataTransferObjects.User;

namespace Shared.DataTransferObjects.Group;

public record GroupDto(
    Guid Id,
    UserDto Admin,
    string OrganizationName,
    IEnumerable<Guid> Members,
    GroupReferral Referral,
    GroupKitchenSettingsDto? Settings,
    PaymentInfoDto? PaymentInfo,
    Guid? SelectedKitchenId);