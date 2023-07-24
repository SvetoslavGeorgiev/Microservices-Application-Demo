namespace PlatformService.Data
{
    using Microsoft.EntityFrameworkCore;
    using PlatformService.Models;

    public static class DbPreparation
    {
        public static async Task PrepPopulationAsync(WebApplication app)
        {
            using (var serviceScope = app.Services.CreateScope())
            {
#pragma warning disable CS8604 // Possible null reference argument.
                await SeedDataAsync(serviceScope.ServiceProvider.GetService<AppDbContext>(), app.Environment.IsProduction());
#pragma warning restore CS8604 // Possible null reference argument.
            }
        }

        private static async Task SeedDataAsync(AppDbContext dbContext, bool IsProduction)
        {
            if (!IsProduction)
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
