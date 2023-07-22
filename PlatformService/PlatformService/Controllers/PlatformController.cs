namespace PlatformService.Controllers
{
    using AutoMapper;
    using Microsoft.AspNetCore.Mvc;
    using PlatformService.Data;
    using PlatformService.Dtos;

    [Route("api/[controller]")]
    [ApiController]
    public class PlatformController : ControllerBase
    {
        private readonly IPlatformRepository repository;
        private readonly IMapper mapper;

        public PlatformController(IPlatformRepository _repository, IMapper _mapper)
        {
            repository = _repository;
            mapper = _mapper;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<PlatformCreateDto>>> GetAllPlatforms() 
        {
            Console.WriteLine("--> Getting Platforms....");

            var platformItems = await repository.GetAllPlatforms();

            var model = mapper.Map<IEnumerable<PlatformReadDto>>(platformItems);

            return Ok(model);
        }
    }
}
