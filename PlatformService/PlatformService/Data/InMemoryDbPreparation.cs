namespace PlatformService.Data
{
    using PlatformService.Models;

    public static class InMemoryDbPreparation
    {
        public static async Task PrepPopulation(WebApplication app)
        {
            using (var serviceScope = app.Services.CreateScope())
            {
                await SeedData(serviceScope.ServiceProvider.GetService<AppDbContext>());
            }
        }

        private static async Task SeedData(AppDbContext dbContext)
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
