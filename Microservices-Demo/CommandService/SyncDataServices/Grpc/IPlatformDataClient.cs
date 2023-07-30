namespace CommandService.SyncDataServices.Grpc
{
    using CommandService.Models;

    public interface IPlatformDataClient
    {
        Task<IEnumerable<Platform>> ReturnAllPlatforms();
    }
}
