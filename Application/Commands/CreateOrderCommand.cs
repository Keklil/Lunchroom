﻿using AutoMapper;
using Contracts;
using Contracts.Repositories;
using Domain.DataTransferObjects.Order;
using Domain.Models;
using MediatR;

namespace Application.Commands
{
    public sealed record CreateOrderCommand(OrderForCreationDto Order) : IRequest<OrderDto>;
    internal sealed class CreateOrderHandler : IRequestHandler<CreateOrderCommand, OrderDto>
    {
        private readonly IRepositoryManager _repository;
        private readonly IMapper _mapper;

        public CreateOrderHandler(IRepositoryManager repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public async Task<OrderDto> Handle(CreateOrderCommand request, CancellationToken cancellationToken)
        {
            var orderEntity = new Order(request.Order.CustomerId, request.Order.MenuId, request.Order.GroupId);

            var menu = await _repository.Menu.GetMenuAsync(request.Order.MenuId, trackChanges: false);

            if (request.Order.LunchSetId != default)
            {
                var lunchSet = menu.GetLunchSetById(request.Order.LunchSetId);
                orderEntity.AddLunchSet(lunchSet, request.Order.LunchSetUnits);   
            }
            
            var orderOptions = request.Order.Options;

            foreach (var item in orderOptions)
            {
                var option = menu.Options.Where(x => x.Id == item.OptionId).SingleOrDefault();
                orderEntity.AddOption(option, item.Units);
            }

            orderEntity.ChangeStatus(1); 

            _repository.Order.CreateOrder(orderEntity);
            await _repository.SaveAsync();

            var orderToReturn = orderEntity.Map();

            return orderToReturn;
        }
    }
}
