using CramerGui.Services;
using Microsoft.AspNetCore.SignalR;
using System.Threading.Tasks;

namespace CramerGui.Hubs
{

    public class TheHub : Hub
    {
        private ILightService _lightService;
        private IRoomService _roomService;
        public TheHub(ILightService lightService, IRoomService roomService)
        {
            _lightService = lightService;
            _roomService = roomService;
        }

        public async Task SwitchLight(int lightId, string mode)
        {
            await _lightService.SwitchLight(lightId, mode == "on" ? LightMode.on : LightMode.off, true);
        }

        public async Task DimLight(int lightId, int percentage)
        {
            await _lightService.DimLight(lightId, percentage);
        }

        public async Task SwitchRoomLight(int roomId, string mode)
        {
            await _lightService.SwitchRoomLight(roomId, mode == "on" ? LightMode.on : LightMode.off);
        }

        public async Task DimRoomLight(int roomId, int percentage)
        {
            await _lightService.DimRoomLight(roomId, percentage);
        }

        public async Task KillLights()
        {
            await _lightService.KillLights();
        }

        public override Task OnConnectedAsync()
        {
            return base.OnConnectedAsync();
        }
    }
}
