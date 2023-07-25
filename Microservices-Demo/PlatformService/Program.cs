namespace PlatformService
{
    using Microsoft.EntityFrameworkCore;
    using PlatformService.AsyncDataServices;
    using PlatformService.Data;
    using PlatformService.SyncDataServices.Http;

    public class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            var connectionString = builder.Configuration.GetConnectionString("PlatformsConnection");


            builder.Services.AddDbContext<AppDbContext>(options =>
            {
                if (builder.Environment.IsDevelopment())
                {
                    Console.WriteLine("--> Using InMemory Db");
                    options.UseInMemoryDatabase("InMemory");
                }
                else if (builder.Environment.IsProduction())
                {
                    Console.WriteLine("--> Using SqlServer Db");
                    options.UseSqlServer(connectionString);
                }
            });


            builder.Services.AddControllers();

            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            builder.Services.AddScoped<IPlatformRepository, PlatformRepository>();
            builder.Services.AddHttpClient<ICommandDataClient, CommandDataClient>();
            builder.Services.AddSingleton<IMessageBusClient, MessageBusClient>();

            builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

            var app = builder.Build();

            await DbPreparation.PrepPopulationAsync(app);

            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }


            //app.UseHttpsRedirection();

            app.UseAuthorization();


            app.MapControllers();

            app.Run();
        }
    }
}