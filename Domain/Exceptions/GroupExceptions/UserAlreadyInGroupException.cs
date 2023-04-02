namespace Domain.Exceptions.GroupExceptions;

public class UserAlreadyInGroupException : BadRequestException
{
    public UserAlreadyInGroupException() : base("Пользователь уже включен в группу")
    {
    }
}