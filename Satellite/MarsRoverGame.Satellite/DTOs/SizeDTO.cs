using System.ComponentModel.DataAnnotations;

namespace MarsRoverGame.Satellite.DTOs
{
    public class SizeDTO
    {
        [Range(2, 50)]
        public int XSize { get; set; }
        [Range(2, 50)]
        public int YSize { get; set; }
    }
}
