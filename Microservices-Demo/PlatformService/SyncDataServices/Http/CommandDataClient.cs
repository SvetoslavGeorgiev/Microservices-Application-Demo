namespace PlatformService.SyncDataServices.Http
{
    using PlatformService.Dtos;
    using System.Text;
    using System.Text.Json;
    using System.Threading.Tasks;

    public class CommandDataClient : ICommandDataClient
    {

        private readonly HttpClient httpClient;
        private readonly IConfiguration configuration;

        public CommandDataClient(HttpClient _httpClient, IConfiguration _configuration)
        {
            httpClient = _httpClient;
            configuration = _configuration;
        }
        public async Task SendPlatformToCommand(PlatformReadDto platformReadDto)
        {
            var httpContent = new StringContent(
                    JsonSerializer.Serialize(platformReadDto),
                    Encoding.UTF8,
                    "application/json");

            var response = await httpClient.PostAsync($"{configuration["CommandService"]}", httpContent);

            if (response.IsSuccessStatusCode)
            {
                Console.WriteLine("--> Sync POST to CommandService was OK!");
            }
            else
            {
                Console.WriteLine("--> Sync POST to CommandService Failed!");
            }
        }
    }
}
