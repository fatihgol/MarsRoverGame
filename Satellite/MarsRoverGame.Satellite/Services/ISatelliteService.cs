using MarsRoverGame.Satellite.DTOs;
using MarsRoverGame.Shared;

namespace MarsRoverGame.Satellite.Services
{
    public interface ISatelliteService
    {
        Task<Response<ConfigDTO>> GetConfig(string name);
        Task<Response<ConfigDTO>> SetConfig(string name,string value);
    }
}
