using Domain.DataTransferObjects.User;

namespace Domain.DataTransferObjects.Group;

public static class GroupMapper
{
    public static GroupDto Map(this Models.Group source)
    {
        return new GroupDto(
            Id: source.Id, 
            AdminId: source.Admin.Id, 
            OrganizationName: source.OrganizationName, 
            Address: source.Address, 
            Members: source.Members.Select(sourceMember => new UserDto
            {
                Id = sourceMember.Id,
                Surname = sourceMember.Surname,
                Name = sourceMember.Name,
                Patronymic = sourceMember.Patronymic,
                Email = sourceMember.Email,
                
            })
        );
    }
}