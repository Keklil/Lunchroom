using Domain.Models;
using Shared.DataTransferObjects.Kitchen;

namespace Shared.DataTransferObjects.Group;

public class AvailableKitchensDto
{
    public string OrganizationName { get; init; }
    public string Address { get; init; }
    public Contacts Contacts { get; init; }
    public KitchenSettingsDto Settings { get; init; }
}