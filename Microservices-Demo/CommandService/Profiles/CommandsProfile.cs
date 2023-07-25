namespace CommandService.Profiles
{
    using AutoMapper;
    using Dtos;
    using Models;

    public class CommandsProfile : Profile
    {
        public CommandsProfile()
        {
            CreateMap<Platform, PlatformReadDto>().ReverseMap();
            CreateMap<Command, CommandReadDto>().ReverseMap();
            CreateMap<Command, CommandCreateDto>().ReverseMap();
            CreateMap<PlatformPublishedDto, Platform>()
                .ForMember(x => x.ExternalID, opt => opt.MapFrom(x => x.ExternalId));
        }
    }
}
