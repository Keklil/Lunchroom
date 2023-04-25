using Domain.Models;
using Domain.Models.Enums;
using NetTopologySuite.Geometries;

namespace Shared.DataTransferObjects.Group;

public record GroupKitchenSettingsDto(
    string Address,
    Point Location);