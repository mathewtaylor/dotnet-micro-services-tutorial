using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using PlatformService.Data;
using PlatformService.DTOs;
using PlatformService.Models;
using PlatformService.Services.AsyncData;
using PlatformService.Services.Http;

namespace PlatformService.Controllers
{
  [Route("api/platforms")]
  [ApiController]
  public class PlatformsController : ControllerBase
  {
    private readonly IPlatformRepo _repository;
    private IMapper _mapper;
    private readonly ICommandDataClient _commandDataClient;
    private readonly IMessageBusClient _messageBusClient;

    public PlatformsController(IPlatformRepo repository, IMapper mapper, ICommandDataClient commandDataClient, IMessageBusClient messageBusClient)
    {
      _repository = repository;
      _mapper = mapper;
      _commandDataClient = commandDataClient;
      _messageBusClient = messageBusClient;
    }

    [HttpGet]
    public ActionResult<IEnumerable<PlatformReadDto>> GetPlatforms()
    {
      var platforms = _repository.GetAllPlatforms();

      return Ok(_mapper.Map<IEnumerable<PlatformReadDto>>(platforms));
    }

    [HttpGet("{id}", Name = "GetPlatformById")]
    public ActionResult<PlatformReadDto> GetPlatformById(int id)
    {
      var platform = _repository.GetPlatformById(id);

      if (platform == null)
      {
        return NotFound();
      }

      return Ok(_mapper.Map<PlatformReadDto>(platform));
    }

    [HttpPost]
    public async Task<ActionResult<PlatformReadDto>> CreatePlatform(PlatformCreateDto platformCreateDto)
    {
      var platform = _mapper.Map<Platform>(platformCreateDto);

      _repository.CreatePlatform(platform);
      _repository.SaveChanges();

      var platformReadDto = _mapper.Map<PlatformReadDto>(platform);

      // Send Sync Message
      try
      {
        await _commandDataClient.SendPlatformToCommand(platformReadDto);
      }
      catch (Exception ex)
      {
        Console.WriteLine($"--> Could not send to sync message Command Service: {ex.Message}");
      }

      // Send Async Message
      try
      {
        var platformPublishedDto = _mapper.Map<PlatformPublishedDto>(platformReadDto);
        platformPublishedDto.Event = "Platform_Published";
        _messageBusClient.PublishNewPlatform(platformPublishedDto);
      }
      catch (Exception ex)
      {
        Console.WriteLine($"--> Could not send to async message Command Service: {ex.Message}");
      }

      return CreatedAtRoute(nameof(GetPlatformById), new { Id = platformReadDto.Id }, platformReadDto);
    }
  }
}