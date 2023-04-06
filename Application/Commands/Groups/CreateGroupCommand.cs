using Contracts;
using Contracts.Repositories;
using Contracts.Security;
using Domain.Exceptions;
using Domain.Exceptions.AuthExceptions;
using Domain.Models;
using MediatR;
using Microsoft.Extensions.Logging;
using Shared.DataTransferObjects.Group;

namespace Application.Commands.Groups;

public sealed record CreateGroupCommand(Guid AdminId, string OrganizationName, string Address) : IRequest<GroupDto>;

internal sealed class CreateGroupHandler : IRequestHandler<CreateGroupCommand, GroupDto>
{
    private readonly ILogger<CreateGroupHandler> _logger;
    private readonly IRepositoryManager _repository;
    private readonly ITokenService _tokenService;

    public async Task<GroupDto> Handle(CreateGroupCommand request, CancellationToken cancellationToken)
    {
        try
        {
            var admin = await _repository.User.GetUserAsync(request.AdminId);
            if (admin is null || admin.Role is not Role.KitchenOperator)
                throw new AttemptCreateGroupByNonAdminException();

            var group = new Group(admin, request.OrganizationName, request.Address);

            var referToken = await _tokenService.GenerateReferral(group);

            group.SetReferralToken(referToken);

            _repository.Groups.CreateGroup(group);
            await _repository.SaveAsync();

            return group.Map();
        }
        catch (AttemptCreateGroupByNonAdminException ex)
        {
            _logger.LogError(ex.ToString());
            throw;
        }
        catch (AttemptSetNullOrEmptyToken ex)
        {
            _logger.LogError(ex.ToString());
            throw;
        }
    }

    public CreateGroupHandler(IRepositoryManager repository, ILogger<CreateGroupHandler> logger, ITokenService tokenService)
    {
        _repository = repository;
        _logger = logger;
        _tokenService = tokenService;
    }
}