using Domain.Models;

namespace Contracts;

public interface ITokenService
{
    public Task<string> Generate(string payload);
    public Task<string> Generate(object payload);
    public Task<string> Generate(User user);
}