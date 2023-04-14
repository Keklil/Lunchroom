namespace Domain.Exceptions.AuthExceptions;

public class AttemptCreateKitchenByNonKitchenOperator : BadRequestException
{
    public AttemptCreateKitchenByNonKitchenOperator() 
        : base("Попытка создать столовую пользователем не в роли оператара столовой") 
    {
    }
}