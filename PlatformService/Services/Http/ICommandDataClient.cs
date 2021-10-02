using System.Threading.Tasks;
using PlatformService.DTOs;

namespace PlatformService.Services.Http
{
  public interface ICommandDataClient
  {
    Task SendPlatformToCommand(PlatformReadDto platform);
  }
}