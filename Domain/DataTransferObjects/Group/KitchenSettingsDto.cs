using Domain.Models;

namespace Domain.DataTransferObjects.Group;

public record KitchenSettingsDto(
    string TargetEmail,
    string KitchenName,
    int HourExpired,
    int MinuteExpired,
    PeriodicRefresh PeriodicRefresh,
    MenuFormat MenuFormat);