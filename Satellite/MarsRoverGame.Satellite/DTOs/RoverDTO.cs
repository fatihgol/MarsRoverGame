using System.Text.Json.Serialization;

namespace MarsRoverGame.Satellite.DTOs
{
    public class RoverDTO
    {
        public string ServiceUrl { get; set; }
        public string RoverId { get; set; }
        public int X { get; set; }
        public int Y { get; set; }
        public string Direction { get; set; }
    }
}
