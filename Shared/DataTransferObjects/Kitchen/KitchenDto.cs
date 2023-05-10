﻿using Domain.Models;
using Shared.DataTransferObjects.User;

namespace Shared.DataTransferObjects.Kitchen;

public record KitchenDto(
    Guid Id,
    string OrganizationName,
    string Address,
    string Inn,
    Contacts Contacts,
    bool AllowForOrders,
    IReadOnlyCollection<UserDto> Managers,
    KitchenSettingsDto? Settings);