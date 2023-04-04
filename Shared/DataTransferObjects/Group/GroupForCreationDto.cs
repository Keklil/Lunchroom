namespace Shared.DataTransferObjects.Group;

public record GroupForCreationDto(Guid AdminId, string OrganizationName, string Address);