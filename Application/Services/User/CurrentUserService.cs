using System.Security.Claims;
using Application.Authorization.Exceptions;
using Contracts.Security;
using Microsoft.AspNetCore.Http;

namespace Application.Services.User;

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

        throw new AuthException("В токене не найден идентификатор пользователя");
    }
}