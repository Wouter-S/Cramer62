using System.Collections.Generic;
using System.Threading.Tasks;

namespace CramerGui.Services
{
    public interface ILightService
    {
        Task DimLight(long lightId, long percentage, bool alert = false);
        Task DimLight(long[] lightIds, long percentage, bool alert = false);
        Task DimLight(long[] lightIds, long percentage, LightMode? lightMode, bool alert = false);
        Task DimRoomLight(long roomId, long percentage);
        List<Light> GetLight();
        Light GetLight(long lightId);
        List<LightLog> GetLightLogs();
        List<Light> GetLightsByRoom(long roomId);
        Task KillLights();
        Task SwitchLight(long lightId, LightMode mode, bool alert = false);
        Task SwitchRoomLight(long roomId, LightMode mode, bool isAutomatic = false);
        Task UpdateLights(List<Light> lights);
        Task Init();

        Task LightsChanged();
        Light GetLight(string friendlyName);
        //void DimLight(long lightId, object value, bool v);
    }
}