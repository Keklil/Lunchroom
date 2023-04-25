using Domain.Models;
using Domain.Models.Enums;
using NetTopologySuite.Geometries;

namespace Shared.DataTransferObjects.Group;

public record GroupConfigDto(
    Guid GroupId,
    string Address,
    Point Location);