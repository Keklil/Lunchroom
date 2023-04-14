using Domain.Infrastructure;

namespace Shared.DataTransferObjects.Group;

public record GroupDto(
    Guid Id,
    Guid AdminId,
    string OrganizationName,
    string Address,
    IEnumerable<Guid> Members,
    GroupReferral Referral,
    GroupKitchenSettingsDto Settings,
    PaymentInfoDto PaymentInfo);