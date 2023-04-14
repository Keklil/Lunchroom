namespace Shared.DataTransferObjects.User;

public class UserDto
{
    public Guid Id { get; init; }
    public string? Surname { get; init; }
    public string? Name { get; init; }
    public string? Patronymic { get; init; }
    public string Email { get; init; } = null!;
    public bool IsEmailChecked { get; set; }
    public bool NameFill { get; set; }

    public List<Guid> Groups { get; set; }
}