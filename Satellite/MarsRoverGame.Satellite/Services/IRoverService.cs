using MarsRoverGame.Satellite.DTOs;
using MarsRoverGame.Shared;

namespace MarsRoverGame.Satellite.Services
{
    public interface IRoverService
    {
        Task<Response<List<RoverDTO>>> GetActiveRoversAsync();
        Task<Response<string>> AddActiveRoverAsync(RoverDTO rover);
        Task<Response<RoverDTO>> GetActiveRoverAsync(string roverId);
        Task<Response<RoverDTO>> SetRoverLocationAsync(string roverId, LocationDTO location);
        Task<Response<string>> RemoveActiveRoverAsync(string roverId);
    }
}
