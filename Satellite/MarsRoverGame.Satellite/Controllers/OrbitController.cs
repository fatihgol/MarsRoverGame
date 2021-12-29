using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MarsRoverGame.Satellite.DTOs;
using MarsRoverGame.Satellite.Services;
using System.Diagnostics;
using System.Text;
using RestSharp;

namespace MarsRoverGame.Satellite.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class OrbitController : ControllerBase
    {

        private readonly IRoverService _roverService;
        private readonly ISatelliteService _satelliteService;

        public OrbitController(IRoverService roverService, ISatelliteService satelliteService)
        {
            _roverService = roverService;
            _satelliteService = satelliteService;
        }

        [HttpGet]
        [ActionName("Ping")]
        public async Task<IActionResult> Ping()
        {
            return new ObjectResult("Satellite connection ok.")
            {
                StatusCode = 200
            };
        }


        [HttpPost]
        [ActionName("SetPlateauSize")]
        public async Task<IActionResult> SetPlateauSize(SizeDTO size)
        {

            var response = await _satelliteService.SetConfig("PlateauSize", $"{size.XSize}x{size.YSize}");

            if (response.IsSuccessful)
            {
                int requiredRoverCount = (int)Math.Ceiling(Math.Sqrt(size.XSize * size.YSize / 25));

                var rovers = await _roverService.GetActiveRoversAsync();

                foreach (var rover in rovers.Data)
                {
                    await DisconnectToSatellite(rover.RoverId);
                }

                int numberOfRoversToBeActive = requiredRoverCount - rovers.Data.Count();
                for (int i = 0; i < requiredRoverCount; i++)
                {
                    await ActivateNewRover();
                }

            }

            return new ObjectResult(response)
            {
                StatusCode = response.StatusCode
            };
        }

        [HttpPost]
        [ActionName("ActivateNewRover")]
        public async Task<IActionResult> ActivateNewRover()
        {
            var rovers = await _roverService.GetActiveRoversAsync();
            int currentRoverCount = rovers.Data.Count;

            if (currentRoverCount < 10)
            {
                string[] str = { "cd ..\\..\\Rover\\MarsRoverGame.Rover", "start /d \".\" dotnet run --urls=https://localhost:761" + currentRoverCount };

                await System.IO.File.WriteAllLinesAsync("StartRover.bat", str);

                Process.Start("StartRover.bat");

                var plateauSizeResponse = await _satelliteService.GetConfig("PlateauSize");


                string[] size = plateauSizeResponse.Data.Value.Split('x');

                Random random = new Random();
                int x = random.Next(0, Convert.ToInt32(size[0]));
                int y = random.Next(0, Convert.ToInt32(size[1]));
                string directions = "NWSE";
                char direction = directions[random.Next(0, 4)];

                await ConnectToSatellite(new RoverDTO { RoverId = Guid.NewGuid().ToString(), ServiceUrl = "https://localhost:761" + currentRoverCount, X = x, Y = y, Direction = direction.ToString() });

                for (int i = 0; i < 4; i++)
                {
                    var client = new RestClient("https://localhost:761" + currentRoverCount + "/api/Discovery/Ping");
                    client.Timeout = -1;
                    var request = new RestRequest(Method.GET);
                    IRestResponse response = await client.ExecuteAsync(request);

                    if (response.StatusCode == System.Net.HttpStatusCode.OK)
                    {
                        return new ObjectResult(new { IsSuccessful = true, Message = "Rover activated." })
                        {
                            StatusCode = 200
                        };
                    }
                }

            }
            else
            {
                return new ObjectResult(new { IsSuccessful = false, Errors = "Maximum count of rovers is 10." })
                {
                    StatusCode = 500
                };
            }


            return new ObjectResult("")
            {
                StatusCode = 200
            };
        }

        [HttpGet]
        [ActionName("GetPlateauSize")]
        public async Task<IActionResult> GetPlateauSize()
        {

            var response = await _satelliteService.GetConfig("PlateauSize");

            return new ObjectResult(response)
            {
                StatusCode = response.StatusCode
            };
        }

        [HttpPost]
        [ActionName("ConnectToSatellite")]
        public async Task<IActionResult> ConnectToSatellite(RoverDTO rover)
        {
            var response = await _roverService.AddActiveRoverAsync(rover);

            return new ObjectResult(response)
            {
                StatusCode = response.StatusCode
            };
        }

        [HttpPost]
        [ActionName("MoveTheRover")]
        public async Task<IActionResult> MoveTheRover(MoveCommandDTO command)
        {
            var roverData = await _roverService.GetActiveRoverAsync(command.RoverId);
            var plateauData = await _satelliteService.GetConfig("PlateauSize");

            string[] plateauSize = plateauData.Data.Value.Split('x');
            int plateauXSize = Convert.ToInt32(plateauSize[0]);
            int plateauYSize = Convert.ToInt32(plateauSize[1]);

            var currentX = roverData.Data.X;
            var currentY = roverData.Data.Y;

            var currentDirection = roverData.Data.Direction;

            string directions = "NWSE";

            foreach (char cmd in command.Command)
            {
                if (cmd == 'L')
                {
                    int currentDirectionIndex = directions.IndexOf(currentDirection);

                    currentDirection = directions[(currentDirectionIndex + 1) % 4].ToString();
                }
                else if (cmd == 'R')
                {
                    int currentDirectionIndex = directions.IndexOf(currentDirection);

                    currentDirection = currentDirectionIndex == 0 ? directions[directions.Length - 1].ToString() : directions[currentDirectionIndex - 1].ToString();
                }
                else if (cmd == 'M')
                {
                    //Check Plateau Limits
                    if ((currentDirection == "N" && currentX == plateauXSize - 1)
                        || (currentDirection == "S" && currentX == 0)
                        || (currentDirection == "E" && currentY == plateauYSize - 1)
                        || (currentDirection == "W" && currentY == 0))
                    {
                        return new ObjectResult(new { Errors = "Plateau limits violated." })
                        {
                            StatusCode = 500
                        };
                    }

                    if (currentDirection == "N")
                        currentX++;
                    else if (currentDirection == "S")
                        currentX--;
                    else if (currentDirection == "E")
                        currentY++;
                    else if (currentDirection == "W")
                        currentY--;
                }
            }

            //Move Rover
            var client = new RestClient(roverData.Data.ServiceUrl + "/api/Discovery/Move?command=" + command.Command);
            client.Timeout = -1;
            var request = new RestRequest(Method.POST);
            IRestResponse response = await client.ExecuteAsync(request);

            if (response.StatusCode == System.Net.HttpStatusCode.OK)
            {
               var setRoverLocationResponse = await _roverService.SetRoverLocationAsync(roverData.Data.RoverId, new LocationDTO { Direction = currentDirection, X = currentX, Y = currentY });

                return new ObjectResult(setRoverLocationResponse)
                {
                    StatusCode = setRoverLocationResponse.StatusCode
                };
            }
            else
            {
                return new ObjectResult(response.Content)
                {
                    StatusCode = (int)response.StatusCode
                };
            }
        }

        [HttpPost]
        [ActionName("DisconnectToSatellite")]
        public async Task<IActionResult> DisconnectToSatellite(string roverId)
        {
            var response = await _roverService.RemoveActiveRoverAsync(roverId);

            return new ObjectResult(response)
            {
                StatusCode = response.StatusCode
            };
        }

        [HttpGet]
        [ActionName("GetActiveRovers")]
        public async Task<IActionResult> GetActiveRovers()
        {
            var response = await _roverService.GetActiveRoversAsync();

            return new ObjectResult(response)
            {
                StatusCode = response.StatusCode
            };
        }

        [HttpGet]
        [ActionName("GetActiveRover")]
        public async Task<IActionResult> GetActiveRover(string roverId)
        {
            var response = await _roverService.GetActiveRoverAsync(roverId);

            return new ObjectResult(response)
            {
                StatusCode = response.StatusCode
            };
        }
    }
}
