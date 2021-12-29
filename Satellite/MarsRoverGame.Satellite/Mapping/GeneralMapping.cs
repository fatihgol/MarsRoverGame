using AutoMapper;
using MarsRoverGame.Satellite.DTOs;
using MarsRoverGame.Satellite.Models;

namespace MarsRoverGame.Satellite.Mapping
{
    public class GeneralMapping : Profile
    {
        public GeneralMapping()
        {
            CreateMap<RoverModel, RoverDTO>().ReverseMap();
            CreateMap<RoverModel, LocationDTO>().ReverseMap();
            CreateMap<ConfigModel, ConfigDTO>().ReverseMap();
        }
    }
}
