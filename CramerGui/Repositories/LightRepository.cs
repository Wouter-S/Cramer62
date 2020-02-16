using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CramerGui.Repositories
{
    public class LightRepository : ILightRepository
    {
        private IServiceProvider _serviceProvider;
        protected IMemoryCache _cache;


        public LightRepository(IServiceProvider serviceProvider, IMemoryCache memoryCache)
        {
            _serviceProvider = serviceProvider;
            _cache = memoryCache;
        }

        public List<Light> GetLights()
        {
            using (var context = new DatabaseContext(_serviceProvider.GetRequiredService<DbContextOptions<DatabaseContext>>()))
            {
                var light = context.Lights.ToList();

                return light;
            }
        }

        public Light GetLight(long lightId)
        {
            using (var context = new DatabaseContext(
                 _serviceProvider.GetRequiredService<DbContextOptions<DatabaseContext>>()))
            {
                var light = context.Lights
                    .Single(s => s.LightId == lightId);

                return light;
            }
        }

        public Light GetLight(string friendlyNameSlug)
        {
            using (var context = new DatabaseContext(
                 _serviceProvider.GetRequiredService<DbContextOptions<DatabaseContext>>()))
            {
                var light = context.Lights
                    .SingleOrDefault(s => s.FriendlyNameSlug.ToLower() == friendlyNameSlug.ToLower());

                return light;
            }
        }


        public List<Light> GetLightsByRoom(long roomId)
        {
            using (var context = new DatabaseContext(
                _serviceProvider.GetRequiredService<DbContextOptions<DatabaseContext>>()))
            {
                var light = context.Lights
                    .Where(l => l.RoomId == roomId)
                    .OrderBy(l => l.LightId)
                    .ToList();

                return light;
            }
        }

        public void SetLights(long lightId, long percentage)
        {
            using (var context = new DatabaseContext(
                _serviceProvider.GetRequiredService<DbContextOptions<DatabaseContext>>()))
            {
                var light = context.Lights
                    .Single(s => s.LightId == lightId);

                light.Percentage = percentage;
                context.SaveChanges();
            }
        }

        public void SetLights(long lightId, LightMode mode)
        {
            using (var context = new DatabaseContext(
                _serviceProvider.GetRequiredService<DbContextOptions<DatabaseContext>>()))
            {
                var light = context.Lights
                    .Single(s => s.LightId == lightId);

                light.Mode = mode;
                context.SaveChanges();
            }
        }

        public void SetLightLog(long lightId, long value)
        {
            if (_cache.Get<List<LightLog>>("lightLog") == null)
            {
                _cache.Set("lightLog", new List<LightLog>());
            }
            _cache.Get<List<LightLog>>("lightLog")?.Add(new LightLog()
            {
                LightId = lightId,
                Value = value,
                Timestamp = DateTime.Now,
            });
        }

        public List<LightLog> GetLightLogs()
        {
            return _cache.Get<List<LightLog>>("lightLog") ?? new List<LightLog>();
        }

        public async Task Update(Light light)
        {
            using (var context = new DatabaseContext(
              _serviceProvider.GetRequiredService<DbContextOptions<DatabaseContext>>()))
            {
                context.Entry(light).State = EntityState.Modified;
                await context.SaveChangesAsync();
            }
        }
    }
}
