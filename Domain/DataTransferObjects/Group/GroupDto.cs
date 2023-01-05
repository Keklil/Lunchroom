using Domain.DataTransferObjects.User;

namespace Domain.DataTransferObjects.Group;

public record GroupDto(Guid Id, Guid AdminId, string OrganizationName, string Address, IEnumerable<UserDto> Members)
{
    
}