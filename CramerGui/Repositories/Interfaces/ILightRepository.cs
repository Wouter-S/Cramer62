using System.Collections.Generic;
using System.Threading.Tasks;

namespace CramerAlexa.Repositories
{
    public interface ILightRepository
    {
        Light GetLight(long lightId);
        List<Light> GetLights();
        List<Light> GetLightsByRoom(long roomId);
        void SetLights(long lightId, LightMode mode);
        void SetLights(long lightId, long percentage);
        Task Update(Light light);
        void SetLightLog(long lightId, long mode);
        List<LightLog> GetLightLogs();
        Light GetLight(string friendlyName);
    }
}