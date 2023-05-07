namespace Domain.Exceptions;

public sealed class UserNotFoundException : NotFoundException
{
    public UserNotFoundException(Guid userId)
        : base("Пользователь с id: {UserId} не существует.", userId)
    {
    }

    public UserNotFoundException(string email)
        : base("Пользователь с почтой: {Email} doesn't exist.", email)
    {
    }
}