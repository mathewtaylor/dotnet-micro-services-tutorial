using System;
using Microsoft.AspNetCore.Mvc;

namespace CommandsService.Controllers
{
  [Route("api/c/platforms")]
  [ApiController]
  public class PlatformsController : ControllerBase
  {
    public PlatformsController()
    {

    }

    [HttpPost]
    public ActionResult TestInboundConnection()
    {
      Console.WriteLine("--> Inbound POST # Command Service");
      return Ok("Inbound test okay from Platforms Controller");
    }
  }
}