namespace Contracts.Security;

public interface ICurrentUserService
{
    Guid GetUserId();
}