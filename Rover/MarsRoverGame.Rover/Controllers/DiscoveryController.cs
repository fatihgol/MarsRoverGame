using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace MarsRoverGame.Rover.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class DiscoveryController : ControllerBase
    {
        [HttpGet]
        [ActionName("Ping")]
        public async Task<IActionResult> Ping()
        {

            return new ObjectResult("Rover connection ok.")
            {
                StatusCode = 200
            };
        }

        [HttpPost]
        [ActionName("Move")]
        public async Task<IActionResult> Move(string command)
        {

            return new ObjectResult("Rover Moved")
            {
                StatusCode = 200
            };
        }


        [HttpPost]
        [ActionName("LeaveRover")]
        public async Task<IActionResult> LeaveRover()
        {
            Environment.Exit(0);
            return new ObjectResult("")
            {
                StatusCode = 200
            };
        }
    }
}
