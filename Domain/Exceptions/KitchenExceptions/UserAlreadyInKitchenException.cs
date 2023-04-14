namespace Domain.Exceptions.KitchenExceptions;

public class UserAlreadyInKitchenException : BadRequestException
{
    public UserAlreadyInKitchenException() : base("Пользователь уже является менеджером столовой")
    {
    }
}