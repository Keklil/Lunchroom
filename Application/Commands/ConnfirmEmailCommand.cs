using Contracts;
using MediatR;

namespace Application.Commands;

public sealed record ConfirmEmailCommand(string Token)
    : IRequest<string>;

internal sealed class ConfirmEmailHandler :
    IRequestHandler<ConfirmEmailCommand, string>
{
    private readonly IAuthService _authService;

    public ConfirmEmailHandler(IAuthService authService)
    {
        _authService = authService;
    }

    public async Task<string> Handle(ConfirmEmailCommand request, CancellationToken cancellationToken)
    {
        var email = await _authService.ConfirmEmail(request.Token);
        
        return email;
    }
}
