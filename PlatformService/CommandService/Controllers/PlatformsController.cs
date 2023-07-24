namespace CommandService.Controllers
{
    using Microsoft.AspNetCore.Mvc;

    [Route("api/c/[controller]")]
    [ApiController]
    public class PlatformsController : ControllerBase
    {
        public PlatformsController()
        {
            
        }

        [HttpPost]
        public async Task<IActionResult> TestInboundConnection()
        {
            Console.WriteLine("--> Inbound POST # Command Service");

            return Ok("Inbound test for the platforms Controller");
        }
    }
}
