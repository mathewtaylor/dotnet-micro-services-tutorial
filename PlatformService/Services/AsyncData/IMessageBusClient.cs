using PlatformService.DTOs;

namespace PlatformService.Services.AsyncData
{
  public interface IMessageBusClient
  {
    void PublishNewPlatform(PlatformPublishedDto platformPublishedDto);
  }
}