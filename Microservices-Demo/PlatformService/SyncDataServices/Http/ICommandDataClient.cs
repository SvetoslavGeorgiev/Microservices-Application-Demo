namespace PlatformService.SyncDataServices.Http
{
    using PlatformService.Dtos;

    public interface ICommandDataClient
    {
        Task SendPlatformToCommand(PlatformReadDto platformReadDto);
    }
}
