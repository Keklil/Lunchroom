using Domain.Models;
using Domain.Models.Enums;

namespace Shared.DataTransferObjects.Kitchen;

public record KitchenSettingsForEditDto(
    TimeSpan LimitingTimeForOrder,
    MenuUpdatePeriod MenuUpdatePeriod,
    MenuFormat MenuFormat);