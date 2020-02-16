using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;

namespace CramerGui.Repositories
{
    public interface ISceneRepository
    {
        Scene CreateScene(Scene @switch);
        //void CreateSwitchAction(SwitchAction action);
        Scene GetScene(long sceneId);
        Scene GetScene(string identifier);
        //void UpdateSwitch(long switchId);
        List<Scene> GetAll();

        void Update(Scene @switch);
        //void UpdateSwitchAction(SwitchAction a);
        //void DeleteSwitchAction(SwitchAction actionToDelete);
        void DeleteScene(Scene switchToDelete);
    }
}