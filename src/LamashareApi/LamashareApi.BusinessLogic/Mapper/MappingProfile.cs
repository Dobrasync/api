using AutoMapper;
using Lamashare.BusinessLogic.Dtos.User;
using LamashareApi.Database.DB.Entities;

namespace Lamashare.BusinessLogic.Mapper;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        CreateMap<User, UserDto>();
    }
}