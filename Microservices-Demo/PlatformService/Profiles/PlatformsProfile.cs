namespace PlatformService.Profiles
{
    using AutoMapper;
    using PlatformService.Dtos;
    using PlatformService.Models;

    public class PlatformsProfile : Profile
    {
        public PlatformsProfile()
        {
            CreateMap<Platform, PlatformReadDto>().ReverseMap();
            CreateMap<Platform, PlatformCreateDto>().ReverseMap();
            CreateMap<PlatformReadDto, PlatformPublishedDto>()
                .ForMember(x => x.ExternalId, opt => opt.MapFrom(x => x.Id));
            CreateMap<Platform, GrpcPlatformModel>()
                .ForMember(x => x.PlatformId, opt => opt.MapFrom(src => src.Id))
                .ForMember(x => x.Name, opt => opt.MapFrom(src => src.Name))
                .ForMember(x => x.Publisher, opt => opt.MapFrom(src => src.Publisher));
        }
    }
}
