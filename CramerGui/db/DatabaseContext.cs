using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CramerAlexa;

namespace CramerAlexa
{
    public class DatabaseContext : DbContext
    {
        public DatabaseContext(DbContextOptions<DatabaseContext> options)
           : base(options)
        {
        }

        public virtual DbSet<CramerAlexa.Light> Lights { get; set; }
        public virtual DbSet<CramerAlexa.LightLog> LightLogs { get; set; }
        public virtual DbSet<CramerAlexa.Room> Rooms { get; set; }
        public virtual DbSet<CramerAlexa.Scene> Scenes { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
        }
    }
}
