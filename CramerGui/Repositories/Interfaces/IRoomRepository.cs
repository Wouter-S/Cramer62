using System.Collections.Generic;

namespace CramerAlexa.Repositories
{
    public interface IRoomRepository
    {
        Room GetRoom(long roomId);
        List<Room> GetRooms();

        Room getRoomBySensor(long sensorId);
    }
}