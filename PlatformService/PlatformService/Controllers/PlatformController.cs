namespace PlatformService.Controllers
{
    using AutoMapper;
    using Microsoft.AspNetCore.Mvc;
    using PlatformService.Data;
    using PlatformService.Dtos;
    using PlatformService.Models;
    using PlatformService.SyncDataServices.Http;

    [Route("api/[controller]")]
    [ApiController]
    public class PlatformController : ControllerBase
    {
        private readonly IPlatformRepository repository;
        private readonly IMapper mapper;
        private readonly ICommandDataClient commandDataClient;

        public PlatformController(
            IPlatformRepository _repository,
            IMapper _mapper,
            ICommandDataClient _commandDataClient)
        {
            repository = _repository;
            mapper = _mapper;
            commandDataClient = _commandDataClient;
        }

        [HttpGet]
        [Route("All")]
        public async Task<IActionResult> GetAllPlatforms() 
        {
            Console.WriteLine("--> Getting Platforms....");

            var platformItems = await repository.GetAllPlatforms();

            var model = mapper.Map<IEnumerable<PlatformReadDto>>(platformItems);

            return Ok(model);
        }

        [HttpGet("{id}", Name = "GetPlatformById")]
        public async Task<IActionResult> GetPlatformById([FromRoute]int id)
        {
            var platformItems = await repository.GetPlatformById(id);

            var model = mapper.Map<PlatformReadDto>(platformItems);

            if (model == null)
            {
                return NotFound();
            }

            return Ok(model);
        }

        [HttpPost]
        public async Task<IActionResult> CreatePlatform(PlatformCreateDto platformCreateDto)
        {
            var platformModel = mapper.Map<Platform>(platformCreateDto);

            //this is to check if going to return correct Id if I swap the database from inMemory to SQL

            //var platdormId = await repository.CreatePlatform(platformModel);
            await repository.CreatePlatform(platformModel);

            await repository.SaveChanges();
            //var platformItems = await repository.GetPlatformById(platdormId);
            //var model = mapper.Map<PlatformReadDto>(platformItems);

            var platdormReadDto = mapper.Map<PlatformReadDto>(platformModel);

            try
            {
                await commandDataClient.SendPlatformToCommand(platdormReadDto);
            }
            catch (Exception e)
            {
                Console.WriteLine($"--> Cloud not send synchronously: {e.Message}");
            }

            return CreatedAtAction(nameof(GetPlatformById), new PlatformReadDto { Id = platdormReadDto.Id }, platdormReadDto);
        }
    }
}
