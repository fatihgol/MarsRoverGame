using AutoMapper;
using MarsRoverGame.Satellite.DTOs;
using MarsRoverGame.Satellite.Models;
using MarsRoverGame.Shared;
using Microsoft.EntityFrameworkCore;

namespace MarsRoverGame.Satellite.Services
{
    public class SatelliteService : ISatelliteService
    {
        private RoverDBContext _roverDBContext;

        private readonly IMapper _mapper;

        public SatelliteService(IMapper mapper, IServiceProvider serviceProvider)
        {
            _roverDBContext = new RoverDBContext(serviceProvider.GetRequiredService<DbContextOptions<RoverDBContext>>());
            _mapper = mapper;
        }

        public async Task<Response<ConfigDTO>> GetConfig(string name)
        {
            try
            {
                return Response<ConfigDTO>.Success(_mapper.Map<ConfigDTO>(_roverDBContext.Configs.FirstOrDefault(x => x.Name == name)),$"{name} config value returned.", 201);
            }
            catch (Exception ex)
            {
                return Response<ConfigDTO>.Fail(ex.ToString(), 500);
            }
        }

        public async Task<Response<ConfigDTO>> SetConfig(string name, string value)
        {
            try
            {
                if(_roverDBContext.Configs.Any(x => x.Name == name))
                {
                    var config = _roverDBContext.Configs.FirstOrDefault(x => x.Name == name);
                    config.Value = value;
                }
                else
                {
                    _roverDBContext.Configs.Add(new ConfigModel(name, value));
                }

                _roverDBContext.SaveChanges();

                return Response<ConfigDTO>.Success(_mapper.Map<ConfigDTO>(_roverDBContext.Configs.FirstOrDefault(x => x.Name == name)), $"{name} config value saved.", 200);
            }
            catch (Exception ex)
            {
                return Response<ConfigDTO>.Fail(ex.ToString(), 500);
            }
        }
    }
}
