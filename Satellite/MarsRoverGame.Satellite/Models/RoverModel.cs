using System.ComponentModel.DataAnnotations;

namespace MarsRoverGame.Satellite.Models
{
    public class RoverModel
    {

        [Key]
        public string RoverId { get; set; }
        public string ServiceUrl { get; set; }
        public int X { get; set; }
        public int Y { get; set; }
        public string Direction { get; set; }
    }
}
