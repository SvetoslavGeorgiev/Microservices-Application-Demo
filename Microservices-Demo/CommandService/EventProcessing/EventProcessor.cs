namespace CommandService.EventProcessing
{
    using AutoMapper;
    using CommandService.Data;
    using CommandService.Dtos;
    using CommandService.Enums;
    using CommandService.Models;
    using System.Text.Json;

    public class EventProcessor : IEventProcessor
    {
        private readonly IServiceScopeFactory scopeFactory;
        private readonly IMapper mapper;

        public EventProcessor(IServiceScopeFactory _scopeFactory, IMapper _mapper)
        {
            scopeFactory = _scopeFactory;
            mapper = _mapper;
        }

        public async Task ProcessEvent(string message)
        {
            var eventType = DetermineEvent(message);

            switch (eventType)
            {
                case EventType.PlatformPublished:
                    await addPlatform(message);
                    break;
                default: break;
            }
        }

        private EventType DetermineEvent(string platformPublishedMessage)
        {
            Console.WriteLine("--> Determining Event");

            var eventType = JsonSerializer.Deserialize<GenericEventDto>(platformPublishedMessage);

            switch (eventType!.Event)
            {
                case "Platform_Published":
                    Console.WriteLine("--> Platform Published Event Detected");
                    return EventType.PlatformPublished;
                default:
                    Console.WriteLine("--> Couldn't determine the event type");
                    return EventType.Undetermined;
            }
        }

        private async Task addPlatform(string platformPublishedMessage)
        {
            using (var scope = scopeFactory.CreateScope())
            {
                var repo = scope.ServiceProvider.GetRequiredService<ICommandRepository>();

                var platformPublishedDto = JsonSerializer.Deserialize<PlatformPublishedDto>(platformPublishedMessage);

                try
                {
                    var platform = mapper.Map<Platform>(platformPublishedDto);

                    if (!repo.IsExternalPlatformExists(platform.ExternalID).Result)
                    {
                        await repo.CreatePlatform(platform);
                        await repo.SaveChanges();
                    }
                    else
                    {
                        Console.WriteLine("--> Platform already exists");
                    }
                }
                catch (Exception e)
                {
                    Console.WriteLine($"--> Couldn't add Platform to DB {e.Message}");
                }

                
            }
        }
    }
}
