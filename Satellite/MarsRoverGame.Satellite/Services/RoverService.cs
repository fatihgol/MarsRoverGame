using AutoMapper;
using MarsRoverGame.Satellite.DTOs;
using MarsRoverGame.Satellite.Models;
using MarsRoverGame.Shared;
using Microsoft.EntityFrameworkCore;

namespace MarsRoverGame.Satellite.Services
{
    public class RoverService : IRoverService
    {
        private RoverDBContext _roverDBContext;

        private readonly IMapper _mapper;

        public RoverService(IMapper mapper, IServiceProvider serviceProvider)
        {
            _roverDBContext = new RoverDBContext(serviceProvider.GetRequiredService<DbContextOptions<RoverDBContext>>());
            _mapper = mapper;
        }

        public async Task<Response<string>> AddActiveRoverAsync(RoverDTO rover)
        {
            try
            {
                _roverDBContext.Rovers.Add(_mapper.Map<RoverModel>(rover));
                _roverDBContext.SaveChanges();

                return Response<string>.Success("Rover has been activated.", "Rover has been activated.", 201);
            }
            catch (Exception ex)
            {
                return Response<string>.Fail(ex.ToString(), 500);
            }
        }

        public async Task<Response<List<RoverDTO>>> GetActiveRoversAsync()
        {
            try
            {
                var activeRovers = _roverDBContext.Rovers.ToList();

                return Response<List<RoverDTO>>.Success(_mapper.Map<List<RoverDTO>>(activeRovers), $"Active rovers listed.(Rovers count: {activeRovers.Count})", 200);
            }
            catch (Exception ex)
            {
                return Response<List<RoverDTO>>.Fail(ex.ToString(), 500);
            }
        }

        public async Task<Response<string>> RemoveActiveRoverAsync(string roverId)
        {
            try
            {
                _roverDBContext.Rovers.Remove(_roverDBContext.Rovers.FirstOrDefault(x => x.RoverId == roverId));

                _roverDBContext.SaveChanges();

                return Response<string>.Success("Rover has been deactivated.", "Rover has been deactivated.", 202);
            }
            catch (Exception ex)
            {
                return Response<string>.Fail(ex.ToString(), 500);
            }
        }

        public async Task<Response<RoverDTO>> GetActiveRoverAsync(string roverId)
        {
            try
            {
                var rover = _roverDBContext.Rovers.FirstOrDefault(x => x.RoverId == roverId);

                return Response<RoverDTO>.Success(_mapper.Map<RoverDTO>(rover), $"{rover.X} {rover.Y} {rover.Direction}", 200);
            }
            catch (Exception ex)
            {
                return Response<RoverDTO>.Fail(ex.ToString(), 500);
            }
        }

        public async Task<Response<RoverDTO>> SetRoverLocationAsync(string roverId, LocationDTO location)
        {
            try
            {
                var rover = _roverDBContext.Rovers.FirstOrDefault(x => x.RoverId == roverId);

                if (_roverDBContext.Rovers.Any(r => r.X == location.X && r.Y == location.Y && r.RoverId != roverId))
                    return Response<RoverDTO>.Fail("A rover already exists at the specified location.", 500);
                else
                {
                    rover.X = location.X;
                    rover.Y = location.Y;

                    rover.Direction = location.Direction;
                }

                _roverDBContext.SaveChanges();

                return Response<RoverDTO>.Success(_mapper.Map<RoverDTO>(rover), $"{location.X} {location.Y} {location.Direction}", 202);
            }
            catch (Exception ex)
            {
                return Response<RoverDTO>.Fail(ex.ToString(), 500);
            }
        }
    }
}
