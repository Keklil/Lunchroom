using Domain.DataTransferObjects.User;
using Domain.Models;

namespace Contracts.Security;

public interface IRegistrationService
{
    public Task<User> RegisterAdmin(UserRegisterDto user);
    public Task<User> RegisterUser(UserRegisterDto user);
}