namespace PlatformService.SyncDataServices.Grpc
{
    using AutoMapper;
    using global::Grpc.Core;
    using PlatformService.Data;

    public class GrpcPlatformService : GrpcPlatform.GrpcPlatformBase
    {
        private readonly IPlatformRepository repository;
        private readonly IMapper mapper;

        public GrpcPlatformService(IPlatformRepository _repository, IMapper _mapper)
        {
            repository = _repository;
            mapper = _mapper;
        }

        public override async Task<PlatformResponse> GetAllPlatforms(GetAllRequest request, ServerCallContext context)
        {
            var response = new PlatformResponse();
            var platforms = await repository.GetAllPlatforms();

            foreach (var platform in platforms)
            {
                response.Platform.Add(mapper.Map<GrpcPlatformModel>(platform));
            }

            return Task.FromResult(response).Result;
        }
    }
}
