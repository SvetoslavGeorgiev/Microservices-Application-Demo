namespace PlatformService.AsyncDataServices
{
    using PlatformService.Dtos;
    using RabbitMQ.Client;
    using System.Text;
    using System.Text.Json;
    using System.Threading.Tasks;

    public class MessageBusClient : IMessageBusClient
    {
        private readonly IConfiguration configuration;
        private readonly IConnection connection;
        private readonly IModel channel;


        public MessageBusClient(IConfiguration _configuration)
        {
            configuration = _configuration;
            var factory = new ConnectionFactory() { HostName = configuration["RabbitMQHost"], Port = int.Parse(configuration["RabbitMQPort"]!) };

            try
            {
                connection = factory.CreateConnection();
                channel = connection.CreateModel();

                channel.ExchangeDeclare(exchange: "trigger", type: ExchangeType.Fanout);

                connection.ConnectionShutdown += RabbitMQ_ConnectionShutdown!;
            }
            catch (Exception e)
            {
                Console.WriteLine($"--> Couldn't connect to Message bus {e.Message}");
            }
        }

        public async Task PublishNewPlatform(PlatformPublishedDto platformPublishedDto)
        {
            var message = JsonSerializer.Serialize(platformPublishedDto);

            if (connection.IsOpen)
            {
                Console.WriteLine("--> RabbitMQ Connection Open, sending message....");
                await SendMessage(message);
            }
            else
            {
                Console.WriteLine("--> RabbitMQ Connection Closed, not sending....");
            }
        }

        private async Task SendMessage(string message)
        {
            var body = Encoding.UTF8.GetBytes(message);

            channel.BasicPublish(exchange: "trigger",
                            routingKey: "",
                            basicProperties: null,
                            body: body);
            await Console.Out.WriteLineAsync($"--> We have sent {message}");
        }

        public void Dispose()
        {
            Console.WriteLine("MessageBus Disposed");
            if (channel.IsOpen)
            {
                channel.Close();
                connection.Close();
            }
        }


        private void RabbitMQ_ConnectionShutdown(object sender, ShutdownEventArgs e)
        {
            Console.WriteLine("--> RabbitMQ Connection Shutdown");
        }
    }
}
