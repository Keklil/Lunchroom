namespace Domain.DataTransferObjects.User;

public static class UserMapper
{
    public static UserDto Map(this Models.User source)
    {
        return new UserDto
        {
            Id = source.Id,
            Surname = source.Surname,
            Name = source.Name,
            Patronymic = source.Patronymic,
            Email = source.Email,
            Groups = source.Groups.Select(x => x.Id).ToList()
        };
    }
    
    
}