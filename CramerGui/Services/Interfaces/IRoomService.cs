using System.Collections.Generic;

namespace CramerGui.Services
{
    public interface IRoomService
    {
        Room GetRoom(long roomId);
        List<Room> GetRooms();

    }
}