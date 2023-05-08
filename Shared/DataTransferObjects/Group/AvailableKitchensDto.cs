using Domain.Models;
using Shared.DataTransferObjects.Kitchen;

namespace Shared.DataTransferObjects.Group;

public record AvailableKitchensDto
{
    public Guid KitchenId { get; init; }
    public string OrganizationName { get; init; }
    public string Address { get; init; }
    public Contacts Contacts { get; init; }
    public KitchenSettingsDto Settings { get; init; }
}