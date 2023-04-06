using System.Security.Claims;
using Contracts.Security;
using Identity.Exceptions;
using Microsoft.AspNetCore.Http;

namespace Identity.Services;

public class CurrentUserService : ICurrentUserService
{
    private readonly IHttpContextAccessor _httpContextAccessor;

    public CurrentUserService(IHttpContextAccessor httpContextAccessor)
    {
        _httpContextAccessor = httpContextAccessor;
    }

    public Guid GetUserId()
    {
        if (Guid.TryParse(_httpContextAccessor.HttpContext?.User?.FindFirstValue("UserID"), out var parsedGuid))
            return parsedGuid;

        throw new AuthException("Invalid guid in token");
    }
}