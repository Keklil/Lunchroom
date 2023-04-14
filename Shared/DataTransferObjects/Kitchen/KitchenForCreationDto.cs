using Domain.Models;

namespace Shared.DataTransferObjects.Kitchen;

public record KitchenForCreationDto(
    string OrganizationName,
    string Address,
    string Inn,
    Contacts Contacts);