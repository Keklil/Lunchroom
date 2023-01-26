using Domain.DataTransferObjects.User;
using Domain.Models;

namespace Contracts.Security;

public interface IAuthService
{
    Task<string> Auth(UserLogin login);
    Task<string> ConfirmEmail(string token);
    Task<User> RegisterAdmin(UserRegisterDto user);
    Task<User> RegisterUser(UserRegisterDto user);
}