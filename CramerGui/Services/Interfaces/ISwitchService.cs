using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CramerAlexa.Services
{
    public interface ISceneService
    {
        //void FlipSwitch(long switchId);
        Task ToggleScene(string switchNumber);
        void CreateSwitch(Scene @switch);
        //void CreateSwitchAction(SwitchAction objSwitch);
        List<Scene> GetAll();
        void Update(List<Scene> switches);
        Task Init();
        //void HandleMqttS witch(string switchName, SwitchClickType? clickAction, bool? occupancy);
    }
}