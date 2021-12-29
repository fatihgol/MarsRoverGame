using System.ComponentModel.DataAnnotations;

namespace MarsRoverGame.Satellite.Models
{
    public class ConfigModel
    {
        [Key]
        public string Name { get; set; }
        public string Value { get; set; }

        public ConfigModel(string name, string value)
        {
            Name = name;
            Value = value;
        }
    }
}
