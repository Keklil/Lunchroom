namespace Domain.Exceptions.AuthExceptions;

public class AttemptCreateKitchenByNonKitchenOperatorException : BadRequestException
{
    public AttemptCreateKitchenByNonKitchenOperatorException() 
        : base("Попытка создать столовую пользователем не в роли оператара столовой") 
    {
    }
}