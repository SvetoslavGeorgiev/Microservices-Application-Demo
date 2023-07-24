namespace PlatformService.Data
{
    using Microsoft.EntityFrameworkCore;
    using PlatformService.Models;
    using System.Collections.Generic;
    using System.Threading.Tasks;

    public class PlatformRepository : IPlatformRepository
    {
        private readonly AppDbContext dbContext;

        public PlatformRepository(AppDbContext _dbContext) 
        {
            dbContext = _dbContext;
        }


        public async Task CreatePlatform(Platform platform)
        {
            if (platform == null) throw new ArgumentNullException(nameof(platform));

            await dbContext.Platforms.AddAsync(platform);
        }

        public async Task<IEnumerable<Platform>> GetAllPlatforms()
            => await dbContext.Platforms.ToListAsync();

        public async Task<Platform> GetPlatformById(int id)
            => await dbContext.Platforms.FirstOrDefaultAsync(p => p.Id.Equals(id));

        public async Task<bool> SaveChanges() => await dbContext.SaveChangesAsync() >= 0;

    }
}
