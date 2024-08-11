using AutoMapper;
using Lamashare.BusinessLogic.Dtos.Library;
using Lamashare.BusinessLogic.Dtos.User;
using LamashareApi.Database.DB.Entities;

namespace Lamashare.BusinessLogic.Mapper.AutoMapper;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        CreateMap<User, UserDto>()
            .ForMember(dest => dest.LibraryIds, opt => opt.MapFrom(src => src.Libraries.Select(x => x.Id)));
        CreateMap<UserCreateDto, User>();
        CreateMap<Library, LibraryDto>();
    }
}