using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CramerAlexa.Services.Interfaces
{
    public interface IHueService
    {
        Task Init();
        Task DimLight(string id, int v2, bool v3);
        Task SwitchLight(string id, LightMode lightMode, bool v2);
        Task<Light> GetLight(string v);
        Task<List<Light>> GetLights();
    }
}
