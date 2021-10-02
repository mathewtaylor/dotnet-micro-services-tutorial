using System;
using System.Text.Json;
using AutoMapper;
using CommandsService.Data;
using CommandsService.DTOs;
using CommandsService.Models;
using Microsoft.Extensions.DependencyInjection;

namespace CommandsService.EventProcessing
{
  public class EventProcessor : IEventProcessor
  {
    private readonly IServiceScopeFactory _scopeFractory;
    private readonly IMapper _mapper;

    public EventProcessor(IServiceScopeFactory scopeFractory, IMapper mapper)
    {
      _scopeFractory = scopeFractory;
      _mapper = mapper;
    }

    public void ProcessEvent(string message)
    {
      var eventType = DetermineEvent(message);

      switch (eventType)
      {
        case EventType.PlatformPublished:
          AddPlatform(message);
          break;
        default:
          break;
      }
    }

    private void AddPlatform(string message)
    {
      using (var scope = _scopeFractory.CreateScope())
      {
        var repo = scope.ServiceProvider.GetRequiredService<ICommandRepo>();

        var platformPublishedDto = JsonSerializer.Deserialize<PlatformPublishedDto>(message);

        try
        {
          var platform = _mapper.Map<Platform>(platformPublishedDto);

          if (!repo.ExternalPlatformExists(platform.ExternalId))
          {
            repo.CreatePlatform(platform);
            repo.SaveChanges();

            Console.WriteLine("Platform added");
          }
          else
          {
            Console.WriteLine($"Platform already exists : {platform.ExternalId}");
          }
        }
        catch (Exception ex)
        {
          Console.WriteLine($"--> Could not add platform to database : {ex.Message}");
        }
      }
    }

    private EventType DetermineEvent(string message)
    {
      Console.WriteLine("--> Determining Event");

      var eventType = JsonSerializer.Deserialize<GenericEventDto>(message);

      switch (eventType.Event)
      {
        case "Platform_Published":
          Console.WriteLine("--> Platform_Published event detected");
          return EventType.PlatformPublished;
        default:
          Console.WriteLine("--> Could not determine event type");
          return EventType.Undetermined;
      }
    }
  }

  enum EventType
  {
    PlatformPublished,
    Undetermined
  }
}