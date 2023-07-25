namespace CommandService.Data
{
    using Microsoft.EntityFrameworkCore;
    using Models;
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;

    public class CommandRepository : ICommandRepository
    {
        private readonly AppDbContext dbContext;

        public CommandRepository(AppDbContext _dbContext) 
        {
            dbContext = _dbContext;
        }

        public async Task CreateCommand(int platformId, Command command)
        {
            if (command == null) throw new ArgumentNullException(nameof(command));

            command.PlatformId = platformId;

            await dbContext.Commands.AddAsync(command);
        }

        public async Task CreatePlatform(Platform platform)
        {
            if (platform == null) throw new ArgumentNullException(nameof(platform));

            await dbContext.Platforms.AddAsync(platform);
        }

        public async Task<IEnumerable<Platform>> GetAllPlatforms()
            => await dbContext.Platforms.ToListAsync();

        public async Task<Command> GetCommand(int platformId, int CommandId)
            => await dbContext.Commands.Where(c => c.PlatformId.Equals(platformId)).FirstOrDefaultAsync();
            

        public async Task<IEnumerable<Command>> GetCommandForPlatform(int platformId)
            => await dbContext.Commands.Where(c => c.PlatformId.Equals(platformId)).OrderBy(c => c.Platform!.Name).ToListAsync();

        public async Task<Platform> GetPlatformById(int platformId)
            => await dbContext.Platforms.FirstOrDefaultAsync(p => p.Id.Equals(platformId));

        public async Task<bool> IsExternalPlatformExists(int externalPlatformId)
            => await dbContext.Platforms.AnyAsync(p => p.ExternalID.Equals(externalPlatformId));

        public async Task<bool> IsPlatformExists(int platformId) 
            => await dbContext.Platforms.AnyAsync(p => p.Id.Equals(platformId));

        public async Task<bool> SaveChanges() => await dbContext.SaveChangesAsync() >= 0;

    }
}
