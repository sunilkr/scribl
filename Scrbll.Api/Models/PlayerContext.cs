using Microsoft.EntityFrameworkCore;

namespace Scrbll.Api.Models
{
    public class PlayerContext : DbContext
    {
        public PlayerContext(DbContextOptions<PlayerContext> opts) : base(opts) { }
        public DbSet<Player> Players { get; set; }
    }
}
