using AutoMapper;
using CommandsService.DTOs;
using CommandsService.Models;

namespace CommandsService.Profiles
{
  public class CommandsProfile : Profile
  {
    public CommandsProfile()
    {
      // Source -> Target
      CreateMap<Platform, PlatformReadDto>();

      CreateMap<CommandCreateDto, Command>();

      CreateMap<Command, CommandReadDto>();

      // Create explicit rule to map Id from source object to ExternalId of the destination object
      CreateMap<PlatformPublishedDto, Platform>()
        .ForMember(dest => dest.ExternalId, opt => opt.MapFrom(src => src.Id));
    }
  }
}