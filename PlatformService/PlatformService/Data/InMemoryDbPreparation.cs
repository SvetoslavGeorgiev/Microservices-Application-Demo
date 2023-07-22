namespace PlatformService.Data
{
    using PlatformService.Models;

    public static class InMemoryDbPreparation
    {
        public static async Task PrepPopulationAsync(WebApplication app)
        {
            using (var serviceScope = app.Services.CreateScope())
            {
#pragma warning disable CS8604 // Possible null reference argument.
                await SeedDataAsync(serviceScope.ServiceProvider.GetService<AppDbContext>());
#pragma warning restore CS8604 // Possible null reference argument.
            }
        }

        private static async Task SeedDataAsync(AppDbContext dbContext)
        {
            if (!dbContext.Platforms.Any())
            {
                Console.WriteLine("--> Seeding Data");

                dbContext.Platforms.AddRange(
                    new Platform() { Name = "Dot Net", Publisher = "Microsoft", Cost = "Free" },
                    new Platform() { Name = "SQL Server Express", Publisher = "Microsoft", Cost = "Free" },
                    new Platform() { Name = "Kubernetis", Publisher = "Cloud Native Computing Foundation", Cost = "Free" });


                await dbContext.SaveChangesAsync();
            }
            else
            {
                Console.WriteLine("--> We already have data");
            }
        }
    }
}
