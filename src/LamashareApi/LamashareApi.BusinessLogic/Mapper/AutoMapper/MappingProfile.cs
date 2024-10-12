using AutoMapper;
using Lamashare.BusinessLogic.Dtos.Library;
using Lamashare.BusinessLogic.Dtos.User;
using LamashareApi.Database.DB.Entities;

namespace Lamashare.BusinessLogic.Mapper.AutoMapper;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        CreateMap<UserEntity, UserDto>()
            .ForMember(dest => dest.LibraryIds, opt => opt.MapFrom(src => src.Libraries.Select(x => x.Id)));
        CreateMap<UserCreateDto, UserEntity>();
        CreateMap<LibraryEntity, LibraryDto>();
        CreateMap<LibraryCreateDto, LibraryEntity>();
    }
}