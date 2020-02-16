using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CramerGui.Repositories
{
    public class RoomRepository : IRoomRepository
    {
        private IServiceProvider _serviceProvider;

        public RoomRepository(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;

        }

        public List<Room> GetRooms()
        {
            using (var context = new DatabaseContext(
                  _serviceProvider.GetRequiredService<DbContextOptions<DatabaseContext>>()))
            {
                var rooms = context.Rooms
                    .Include(r => r.RoomLights)
                    //.Include(r=>r.RoomSensors)
                    //.ThenInclude(a=>a.Sensor)
                    .ToList();
                return rooms;
            }
        }

        public Room GetRoom(long roomId)
        {
            using (var context = new DatabaseContext(
               _serviceProvider.GetRequiredService<DbContextOptions<DatabaseContext>>()))
            {
                var room = context.Rooms
                    .Where(s => s.RoomId == roomId)
                    .Include(s => s.RoomLights)
                    //.ThenInclude(a => a)
                    .Single();
                return room;
            }

        }

        public Room getRoomBySensor(long sensorId)
        {

            throw new NotImplementedException();
            //Entities e = new Entities();
            //return e.tblRooms.Where(r => r.tblSensors.Any(a => a.sensorId == sensorId))
            //    .Select(r => new Room
            //    {
            //        RoomId = r.roomId,
            //        Name = r.name,
            //        GroupLights = r.groupLights

            //    }).Single();
        }

    }
}
