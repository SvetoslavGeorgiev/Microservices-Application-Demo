namespace PlatformService.Data
{
    using PlatformService.Models;

    public interface IPlatformRepository
    {
        Task<bool> SaveChanges();

        Task<IEnumerable<Platform>> GetAllPlatforms();

        Task<Platform> GetPlatformById(int id);

        Task CreatePlatform(Platform platform);
    }
}
