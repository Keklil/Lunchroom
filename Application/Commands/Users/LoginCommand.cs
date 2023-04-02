using Contracts.Security;
using Domain.DataTransferObjects.User;
using MediatR;

namespace Application.Commands;

public sealed record LoginCommand(UserLogin Login)
    : IRequest<string>;

internal sealed class LoginCommandHandler : IRequestHandler<LoginCommand, string>
{
    private readonly IAuthService _authService;

    public async Task<string> Handle(LoginCommand request, CancellationToken cancellationToken)
    {
        var token = await _authService.Auth(request.Login);

        return token;
    }

    public LoginCommandHandler(IAuthService authService)
    {
        _authService = authService;
    }
}