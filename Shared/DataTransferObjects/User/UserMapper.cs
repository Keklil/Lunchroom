namespace Shared.DataTransferObjects.User;

public static class UserMapper
{
    public static UserDto Map(this Domain.Models.User source)
    {
        return new UserDto
        {
            Id = source.Id,
            Surname = source.Surname,
            Name = source.Name,
            Patronymic = source.Patronymic,
            Email = source.Email,
            IsEmailChecked = source.IsEmailChecked,
            Groups = source.Groups.Select(x => x.Id).ToList(),
            NameFill = !string.IsNullOrWhiteSpace(source.Name) && !string.IsNullOrWhiteSpace(source.Surname)
        };
    }
}