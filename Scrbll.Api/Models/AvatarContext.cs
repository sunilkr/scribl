using Microsoft.EntityFrameworkCore;

namespace Scrbll.Api.Models
{
    public class AvatarContext : DbContext
    {
        public AvatarContext(DbContextOptions<AvatarContext> opts) : base(opts) { }
        public DbSet<Avatar> Avatars { get; set; }
    }
}
