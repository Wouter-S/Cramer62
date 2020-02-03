using System;
using System.Collections.Generic;
using CramerAlexa.Repositories;

namespace CramerAlexa.Services
{
    public class RoomService : IRoomService
    {
        private IRoomRepository _roomRepo;

        public RoomService(IRoomRepository roomRepository)
        {
            _roomRepo = roomRepository;
        }

        public Room GetRoom(long roomId)
        {
            Room room = _roomRepo.GetRoom(roomId);
            return room;
        }

        public List<Room> GetRooms()
        {
            List<Room> rooms = _roomRepo.GetRooms();
            return rooms;
        }
    }
}
