namespace CommandService.Data
{
    using Models;
    using System.Threading.Tasks;

    public interface ICommandRepository
    {
        Task<bool> SaveChanges();

        Task<IEnumerable<Platform>> GetAllPlatforms();

        Task<bool> IsPlatformExists(int platformId);
        Task<bool> IsExternalPlatformExists(int externalPlatformId);

        Task CreatePlatform(Platform platform);

        Task<IEnumerable<Command>> GetCommandForPlatform(int platformId);

        Task<Command> GetCommand(int platformId, int CommandId);

        Task CreateCommand(int platformId, Command command);
    }
}
