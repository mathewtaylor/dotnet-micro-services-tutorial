using System.Collections.Generic;
using CommandsService.Models;

namespace CommandsService.Services.Grpc
{
  public interface IPlatformDataClient
  {
    IEnumerable<Platform> ReturnAllPlatforms();
  }
}