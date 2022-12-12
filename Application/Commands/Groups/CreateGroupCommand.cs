using AutoMapper;
using Contracts;
using Contracts.Repositories;
using Domain.Exceptions;
using Domain.Models;
using MediatR;

namespace Application.Commands.Groups;

public sealed record CreateGroupCommand(User admin) : IRequest<Group?>;

internal sealed class CreateGroupHandler : IRequestHandler<CreateGroupCommand, Group?>
{
    private readonly IRepositoryManager _repository;
    private readonly IMapper _mapper;
    private readonly ILoggerManager _logger;

    public CreateGroupHandler(IRepositoryManager repository, IMapper mapper, ILoggerManager logger)
    {
        _repository = repository;
        _mapper = mapper;
        _logger = logger;
    }

    public async Task<Group?> Handle(CreateGroupCommand request, CancellationToken cancellationToken)
    {
        try
        {
            var group = new Group(request.admin);

            return group;
        }
        catch (AttemptCreateGroupByNonAdmin ex)
        {
            _logger.LogError(ex.ToString());
            return null;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex.ToString());
            return null;
        }
    }
}