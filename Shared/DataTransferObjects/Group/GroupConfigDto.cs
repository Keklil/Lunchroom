using Domain.Models;
using Domain.Models.Enums;

namespace Shared.DataTransferObjects.Group;

public record GroupConfigDto(
    Guid GroupId,
    string KitchenName,
    int HourExpired,
    int MinuteExpired,
    MenuUpdatePeriod Refresh,
    MenuFormat MenuFormat,
    string TargetEmail);