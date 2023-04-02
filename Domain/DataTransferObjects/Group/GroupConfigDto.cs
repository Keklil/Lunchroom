using Domain.Models;

namespace Domain.DataTransferObjects.Group;

public record GroupConfigDto(
    Guid GroupId,
    string KitchenName,
    int HourExpired,
    int MinuteExpired,
    PeriodicRefresh Refresh,
    MenuFormat MenuFormat,
    string TargetEmail);