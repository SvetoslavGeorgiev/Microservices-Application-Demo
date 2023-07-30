namespace CommandService.Profiles
{
    using AutoMapper;
    using Dtos;
    using Models;
    using PlatformService;

    public class CommandsProfile : Profile
    {
        public CommandsProfile()
        {
            CreateMap<Platform, PlatformReadDto>().ReverseMap();
            CreateMap<Command, CommandReadDto>().ReverseMap();
            CreateMap<Command, CommandCreateDto>().ReverseMap();
            CreateMap<PlatformPublishedDto, Platform>()
                .ForMember(x => x.ExternalID, opt => opt.MapFrom(x => x.ExternalId));
            CreateMap<GrpcPlatformModel, Platform>()
                .ForMember(x => x.ExternalID, opt => opt.MapFrom(src => src.PlatformId))
                .ForMember(x => x.Name, opt => opt.MapFrom(src => src.Name))
                .ForMember(x => x.Commands, opt => opt.Ignore());
        }
    }
}
