using AutoMapper;
using Entities.Models;
using Entities.DataTransferObjects;
namespace LunchRoom
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<User, UserDto>().ReverseMap();
            CreateMap<UserForCreationDto, User>();
            CreateMap<Menu, MenuDto>();
            CreateMap<LunchSet, LunchSetDto>();
            CreateMap<Option, OptionDto>();
            CreateMap<Order, OrderDto>();
            CreateMap<OrderOption, OrderOptionDto>();
            CreateMap<Menu, MenuForList>();
        }
    }
}
