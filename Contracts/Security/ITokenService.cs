using Domain.Models;

namespace Contracts;

public interface ITokenService
{
    Task<string> GenerateReferral(Group group);
    Task<string> Generate(string payload);
    Task<string> Generate(object payload);
    Task<string> Generate(User user);
}