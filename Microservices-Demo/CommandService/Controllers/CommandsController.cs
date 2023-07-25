namespace CommandService.Controllers
{
    using AutoMapper;
    using Data;
    using Dtos;
    using Models;
    using Microsoft.AspNetCore.Mvc;
    using System;

    [Route("api/c/platforms/{platformId}/[controller]")]
    [ApiController]
    public class CommandsController : ControllerBase
    {
        private readonly ICommandRepository commandRepository;
        private readonly IMapper mapper;

        public CommandsController(ICommandRepository _commandRepository, IMapper _mapper)
        {
            commandRepository = _commandRepository;
            mapper = _mapper;
        }

        [HttpGet]
        public async Task<IActionResult> GetCommandsForPlatform(int platformId)
        {
            Console.WriteLine($"--> Hit GetCommandsForPlatform: {platformId}");

            if (!commandRepository.IsPlatformExists(platformId).Result) return NotFound();

            var commands = await commandRepository.GetCommandForPlatform(platformId);

            var model = mapper.Map<IEnumerable<CommandReadDto>>(commands);

            return Ok(model);
        }

        [HttpGet("{commandId}", Name = "GetCommandForPlatform")]
        public async Task<IActionResult> GetCommandForPlatform(int platformId, int commandId)
        {
            Console.WriteLine($"--> Hit GetCommandForPlatform: {platformId} / {commandId}");

            if (!commandRepository.IsPlatformExists(platformId).Result) return NotFound();

            var commands = await commandRepository.GetCommand(platformId, commandId);

            if (commands == null) return NotFound();
            
            var model = mapper.Map<CommandReadDto>(commands);

            return Ok(model);
        }

        [HttpPost]
        public async Task<IActionResult> CreateCommandForPlatform(int platformId, CommandCreateDto commandCreateDto)
        {
            Console.WriteLine($"--> Hit GetCommandsForPlatform: {platformId}");

            if (!commandRepository.IsPlatformExists(platformId).Result) return NotFound();

            var command = mapper.Map<Command>(commandCreateDto);

            await commandRepository.CreateCommand(platformId, command);

            await commandRepository.SaveChanges();

            var commandReadDto = mapper.Map<CommandReadDto>(command);

            return CreatedAtAction(nameof(GetCommandForPlatform), new { platformId = platformId, commandId = commandReadDto.Id }, commandReadDto);
        }
    }
}
