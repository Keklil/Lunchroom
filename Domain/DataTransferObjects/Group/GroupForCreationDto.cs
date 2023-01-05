namespace Domain.DataTransferObjects.Group;

public record GroupForCreationDto(Guid AdminId, string OrganizationName, string Address);
