using System;
using System.Collections.Generic;
using AutoMapper;
using CommandsService.Data;
using CommandsService.DTOs;
using Microsoft.AspNetCore.Mvc;

namespace CommandsService.Controllers
{
  [Route("api/c/platforms")]
  [ApiController]
  public class PlatformsController : ControllerBase
  {
    private readonly ICommandRepo _repository;
    private IMapper _mapper;

    public PlatformsController(ICommandRepo repository, IMapper mapper)
    {
      _repository = repository;
      _mapper = mapper;
    }

    [HttpGet]
    public ActionResult<IEnumerable<PlatformReadDto>> GetPlatforms()
    {
      Console.WriteLine("--> Getting platforms from CommandService");

      var platforms = _repository.GetAllPlatforms();

      return Ok(_mapper.Map<IEnumerable<PlatformReadDto>>(platforms));
    }

    [HttpPost]
    public ActionResult TestInboundConnection()
    {
      Console.WriteLine("--> Inbound POST # Command Service");
      return Ok("Inbound test okay from Platforms Controller");
    }
  }
}