namespace CommandService.Controllers
{
    using AutoMapper;
    using CommandService.Data;
    using CommandService.Dtos;
    using Microsoft.AspNetCore.Mvc;

    [Route("api/c/[controller]")]
    [ApiController]
    public class PlatformsController : ControllerBase
    {
        private readonly ICommandRepository commandRepository;
        private readonly IMapper mapper;

        public PlatformsController(ICommandRepository _commandRepository, IMapper _mapper)
        {
            commandRepository = _commandRepository;
            mapper = _mapper;
        }

        [HttpGet]
        public async Task<IActionResult> GetPlatforms()
        {
            Console.WriteLine("--> Geting platforms from CommandServoce");

            var platformsItems = await commandRepository.GetAllPlatforms();

            var model = mapper.Map<IEnumerable<PlatformReadDto>>(platformsItems);

            return Ok(model);
        }


        [HttpPost]
        public async Task<IActionResult> TestInboundConnection()
        {
            await Console.Out.WriteLineAsync("--> Inbound POST # Command Service");

            return Ok("Inbound test for the platforms Controller");
        }
    }
}
