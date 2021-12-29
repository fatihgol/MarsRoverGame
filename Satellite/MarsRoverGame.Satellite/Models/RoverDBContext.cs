using Microsoft.EntityFrameworkCore;

namespace MarsRoverGame.Satellite.Models
{
    public class RoverDBContext : DbContext
    {
        public RoverDBContext(DbContextOptions<RoverDBContext> options)
        : base(options) { }

        public DbSet<RoverModel> Rovers { get; set; }
        public DbSet<ConfigModel> Configs { get; set; }
    }
}
