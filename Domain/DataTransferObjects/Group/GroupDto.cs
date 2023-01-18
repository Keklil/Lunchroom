using Domain.DataTransferObjects.User;
using Domain.Infrastructure;
using Domain.Models;

namespace Domain.DataTransferObjects.Group;

public record GroupDto(
    Guid Id, 
    Guid AdminId, 
    string OrganizationName, 
    string Address, 
    IEnumerable<Guid> Members, 
    GroupReferral Referral,
    KitchenSettingsDto Settings,
    PaymentInfoDto PaymentInfo);