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
            Members: source.Members.Select(sourceMember => sourceMember.Id),
            Referral: source.Referral
        );
    }
}