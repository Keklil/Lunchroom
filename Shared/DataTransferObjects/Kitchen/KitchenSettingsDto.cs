using Domain.Models;
using Domain.Models.Enums;

namespace Shared.DataTransferObjects.Kitchen;

public record KitchenSettingsDto(
    TimeSpan LimitingTimeForOrder,
    MenuUpdatePeriod MenuUpdatePeriod,
    MenuFormat MenuFormat,
    IReadOnlyList<ShippingArea> ShippingAreas);