namespace PlatformService.AsyncDataServices
{
    using PlatformService.Dtos;

    public interface IMessageBusClient
    {
        Task PublishNewPlatform(PlatformPublishedDto platformPublishedDto);
    }
}
