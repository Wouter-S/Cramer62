using Microsoft.EntityFrameworkCore;

namespace CramerGui
{
    public class DatabaseContext : DbContext
    {
        public DatabaseContext(DbContextOptions<DatabaseContext> options)
           : base(options)
        {
        }

        public virtual DbSet<Light> Lights { get; set; }
        public virtual DbSet<LightLog> LightLogs { get; set; }
        public virtual DbSet<Room> Rooms { get; set; }
        public virtual DbSet<Scene> Scenes { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
        }
    }
}
