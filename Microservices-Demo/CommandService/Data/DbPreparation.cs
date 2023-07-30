namespace CommandService.Data
{
    using CommandService.SyncDataServices.Grpc;
    using Microsoft.EntityFrameworkCore;
    using Models;

    public static class DbPreparation
    {
        public static async Task PrepPopulationAsync(WebApplication app)
        {
            using (var serviceScope = app.Services.CreateScope())
            {
                var grpcClient = serviceScope.ServiceProvider.GetService<IPlatformDataClient>();
#pragma warning disable CS8604 // Possible null reference argument.
                var platforms = await grpcClient!.ReturnAllPlatforms();
                await SeedDataAsync(
                    serviceScope.ServiceProvider.GetService<AppDbContext>(), 
                    app.Environment.IsProduction(), 
                    serviceScope.ServiceProvider.GetService<ICommandRepository>(), 
                    platforms);
#pragma warning restore CS8604 // Possible null reference argument.
            }
        }

        private static async Task SeedDataAsync(
            AppDbContext dbContext, 
            bool IsProduction, 
            ICommandRepository commandRepository, 
            IEnumerable<Platform> platforms)
        {
            if (IsProduction)
            {
                Console.WriteLine("--> Attempting to apply migrations....");
                try
                {
                    dbContext.Database.Migrate();
                }
                catch (Exception e)
                {
                    Console.WriteLine($"--> Couldn't run migrations {e.Message}");
                }

            }
            Console.WriteLine("Seeding new platforms...");

            foreach (var plat in platforms)
            {
                if (!commandRepository.IsExternalPlatformExists(plat.ExternalID).Result)
                {
                   await commandRepository.CreatePlatform(plat);
                }
                await commandRepository.SaveChanges();
            }
        }
    }
}
