using AutoMapper;
using Contracts;
using Contracts.Repositories;
using Domain.DataTransferObjects.Menu;
using Domain.Models;
using MediatR;

namespace Application.Commands
{
    public sealed record CreateMenuCommand(MenuForCreationDto Menu, Guid GroupId): IRequest<MenuDto>;
    internal sealed class CreateMenuHandler: IRequestHandler<CreateMenuCommand, MenuDto>
    {
        private readonly IRepositoryManager _repository;
        private readonly IMapper _mapper;
        private readonly ILoggerManager _logger;

        public CreateMenuHandler(IRepositoryManager repository, IMapper mapper, ILoggerManager logger)
        {
            _repository = repository;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<MenuDto> Handle(CreateMenuCommand request, CancellationToken cancellationToken)
        {
            var dateToday = DateTime.SpecifyKind(DateTime.Now, DateTimeKind.Utc);
            var todayMenu = await _repository.Menu.GetMenuByDateAsync(dateToday, request.GroupId);
            if (todayMenu is not null)
            {
                _logger.LogWarning("Menu has already been uploaded");
                return null;
            }
            
            var menuEntity = new Menu(request.GroupId);
            foreach(var item in request.Menu.LunchSets)
            {
                menuEntity.AddLunchSet(item.Price, item.LunchSetList);
            }
            foreach(var item in request.Menu.Options)
            {
                menuEntity.AddOption(item.Name, item.Price);
            }
            
            _repository.Menu.CreateMenu(menuEntity);
            await _repository.SaveAsync();

            var menuToReturn = _mapper.Map<MenuDto>(menuEntity);

            return menuToReturn;
        }
    }
}
