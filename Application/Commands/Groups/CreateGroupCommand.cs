using AutoMapper;
using Contracts;
using Contracts.Repositories;
using Domain.DataTransferObjects.Group;
using Domain.Exceptions;
using Domain.Models;
using MediatR;

namespace Application.Commands.Groups;

public sealed record CreateGroupCommand(Guid AdminId, string OrganizationName, string Address) : IRequest<GroupDto?>;

internal sealed class CreateGroupHandler : IRequestHandler<CreateGroupCommand, GroupDto?>
{
    private readonly IRepositoryManager _repository;
    private readonly IMapper _mapper;
    private readonly ILoggerManager _logger;
    private readonly ITokenService _tokenService;

    public CreateGroupHandler(IRepositoryManager repository, IMapper mapper, ILoggerManager logger, ITokenService tokenService)
    {
        _repository = repository;
        _mapper = mapper;
        _logger = logger;
        _tokenService = tokenService;
    }

    public async Task<GroupDto?> Handle(CreateGroupCommand request, CancellationToken cancellationToken)
    {
        try
        {
            var admin = await _repository.User.GetUserAsync(request.AdminId, true);
            if (admin.Role is not Role.Admin)
                throw new AttemptCreateGroupByNonAdmin();

            var group = new Group(admin, request.OrganizationName, request.Address);

            var referToken = await _tokenService.GenerateReferral(group);

            group.SetReferralToken(referToken);
            
            _repository.Groups.CreateGroup(group);
            await _repository.SaveAsync();

            return group.Map();
        }
        catch (AttemptCreateGroupByNonAdmin ex)
        {
            _logger.LogError(ex.ToString());
            throw;
        }
        catch (AttemptSetNullOrEmptyToken ex)
        {
            _logger.LogError(ex.ToString());
            throw;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex.ToString());
            throw;
        }
    }
}