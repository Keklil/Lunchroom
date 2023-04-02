namespace Domain.Exceptions;

public sealed class UserNotFoundException : NotFoundException
{
    public UserNotFoundException(Guid userId)
        : base($"The user with id: {userId} doesn't exist.")
    {
    }

    public UserNotFoundException(string email)
        : base($"The user with email: {email} doesn't exist.")
    {
    }
}