namespace CommandService.SyncDataServices.Grpc
{
    using AutoMapper;
    using CommandService.Models;
    using global::Grpc.Net.Client;
    using PlatformService;
    using System.Collections.Generic;
    using System.Threading.Tasks;

    public class PlatformDataClient : IPlatformDataClient
    {
        private readonly IConfiguration configuration;
        private readonly IMapper mapper;

        public PlatformDataClient(IConfiguration _configuration, IMapper _mapper)
        {
            configuration = _configuration;
            mapper = _mapper;
        }
        public async Task<IEnumerable<Platform>> ReturnAllPlatforms()
        {
            Console.WriteLine($"--> Calling GRPC Service {configuration["GrpcPlatform"]}");
            var channel = GrpcChannel.ForAddress(configuration["GrpcPlatform"]!);
            var client = new GrpcPlatform.GrpcPlatformClient(channel);
            var request = new GetAllRequest();

            try
            {
                var reply = client.GetAllPlatforms(request);
                return mapper.Map<IEnumerable<Platform>>(reply.Platform);
            }
            catch (Exception e)
            {
                Console.WriteLine($"--> Couldnot call GRPC Server {e.Message}");
                return null;
            }
        }
    }
}
