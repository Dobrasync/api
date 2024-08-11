using AutoMapper;
using Lamashare.BusinessLogic.Dtos.Library;
using Lamashare.BusinessLogic.Dtos.User;
using LamashareApi.Database.DB.Entities;

namespace Lamashare.BusinessLogic.Mapper.AutoMapper;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        CreateMap<User, UserDto>();
        CreateMap<Library, LibraryDto>();
    }
}