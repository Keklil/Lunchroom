using Contracts;
using Contracts.Repositories;
using Contracts.Security;
using Domain.Exceptions;
using Domain.Exceptions.AuthExceptions;
using Domain.Models;
using Domain.Models.Enums;
using MediatR;
using Microsoft.Extensions.Logging;
using Shared.DataTransferObjects.Group;

namespace Application.Commands.Groups;

public sealed record CreateGroupCommand(string OrganizationName) : IRequest<GroupDto>;

internal sealed class CreateGroupHandler : IRequestHandler<CreateGroupCommand, GroupDto>
{
    private readonly ILogger<CreateGroupHandler> _logger;
    private readonly IRepositoryManager _repository;
    private readonly ITokenService _tokenService;
    private readonly ICurrentUserService _currentUserService;

    public async Task<GroupDto> Handle(CreateGroupCommand request, CancellationToken cancellationToken)
    {
        try
        {
            var currentUserId = _currentUserService.GetUserId();
            var admin = await _repository.User.GetUserAsync(currentUserId);
            if (admin is null || admin.Role is not Role.Customer)
                throw new AttemptCreateGroupByNonCustomerException();

            var group = new Group(admin, request.OrganizationName);

            var referToken = await _tokenService.GenerateReferral(group);

            group.SetReferralToken(referToken);

            _repository.Groups.CreateGroup(group);
            await _repository.SaveAsync(cancellationToken);

            return group.Map();
        }
        catch (AttemptCreateGroupByNonCustomerException ex)
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

    public CreateGroupHandler(IRepositoryManager repository, ILogger<CreateGroupHandler> logger, ITokenService tokenService, ICurrentUserService currentUserService)
    {
        _repository = repository;
        _logger = logger;
        _tokenService = tokenService;
        _currentUserService = currentUserService;
    }
}