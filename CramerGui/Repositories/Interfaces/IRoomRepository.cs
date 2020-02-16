using System.Collections.Generic;

namespace CramerGui.Repositories
{
    public interface IRoomRepository
    {
        Room GetRoom(long roomId);
        List<Room> GetRooms();

        Room getRoomBySensor(long sensorId);
    }
}