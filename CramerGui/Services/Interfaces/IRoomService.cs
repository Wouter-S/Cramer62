using System.Collections.Generic;

namespace CramerAlexa.Services
{
    public interface IRoomService
    {
        Room GetRoom(long roomId);
        List<Room> GetRooms();

    }
}