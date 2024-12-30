using AutoMapper;
using Dobrasync.Api.BusinessLogic.Dtos.Library;
using Dobrasync.Api.BusinessLogic.Dtos.User;
using Dobrasync.Api.Database.DB.Entities;

namespace Dobrasync.Api.BusinessLogic.Mapper.AutoMapper;

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