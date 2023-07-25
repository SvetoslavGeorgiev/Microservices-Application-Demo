namespace CommandService.AsyncDataServices
{
    using CommandService.EventProcessing;
    using RabbitMQ.Client;
    using RabbitMQ.Client.Events;
    using System.Text;
    using System.Threading;
    using System.Threading.Tasks;

    public class MessageBusSubscriber : BackgroundService
    {
        private readonly IConfiguration configuration;
        private readonly IEventProcessor eventProcessor;
        private IConnection connection;
        private IModel channel;
        private string queueName;

        public MessageBusSubscriber(IConfiguration _configuration, IEventProcessor _eventProcessor)
        {
            configuration = _configuration;
            eventProcessor = _eventProcessor;
            InitializeRabbitMQ();
        }

        private void InitializeRabbitMQ()
        {
            var factory = new ConnectionFactory() { HostName = configuration["RabbitMQHost"], Port = int.Parse(configuration["RabbitMQPort"]!) };

            try
            {
                connection = factory.CreateConnection();
                channel = connection.CreateModel();
                channel.ExchangeDeclare(exchange: "trigger", type: ExchangeType.Fanout);
                queueName = channel.QueueDeclare().QueueName;
                channel.QueueBind(queue: queueName,
                    exchange: "trigger", routingKey: "");

                Console.WriteLine("--> Listening on the message Bus....");

                connection.ConnectionShutdown += RabbitMQ_ConnectionShutdown!;
            }
            catch (Exception e)
            {
                Console.WriteLine($"--> Couldn't connect to Message bus {e.Message}");
            }
        }

        protected override Task ExecuteAsync(CancellationToken stoppingToken)
        {
            stoppingToken.ThrowIfCancellationRequested();

            var consumer = new EventingBasicConsumer(channel);

            consumer.Received += (ModuleHandle, ea) =>
            {
                Console.WriteLine("--> Event Received!");

                var body = ea.Body;

                var notificationMessage = Encoding.UTF8.GetString(body.ToArray());

                eventProcessor.ProcessEvent(notificationMessage);
            };

            channel.BasicConsume(queue: queueName, autoAck: true, consumer: consumer);

            return Task.CompletedTask;
        }

        private void RabbitMQ_ConnectionShutdown(object sender, ShutdownEventArgs e)
        {
            Console.WriteLine("--> RabbitMQ Connection Shutdown");
        }

        public override void Dispose()
        {
            if (channel.IsOpen)
            {
                channel.Close();
                connection.Close();
            }
            base.Dispose();
        }
    }
}
