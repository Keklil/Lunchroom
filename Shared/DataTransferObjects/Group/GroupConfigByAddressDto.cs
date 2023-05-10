namespace Shared.DataTransferObjects.Group;

public record GroupConfigByAddressDto(
    Guid GroupId,
    AddressDto Address);