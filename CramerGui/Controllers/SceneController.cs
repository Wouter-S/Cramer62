using CramerGui.Services;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CramerGui.Controllers
{
    public class SceneController : Controller
    {
        private ISceneService _sceneService;
        private ILightService _lightService;

        public SceneController(ISceneService sceneService, ILightService lightService)
        {
            _sceneService = sceneService;
            _lightService = lightService;
        }

        [HttpGet]
        [Route("api/scene/kill")]
        public ActionResult KillSwitch()
        {
            _lightService.KillLights();
            return Ok("");
        }

        [HttpGet]
        [Route("scene/flip/{identifier}")]
        [Route("api/scene/flip/{identifier}")]
        public async Task<ActionResult> ToggleScene(string identifier)
        {
            Console.WriteLine("Flipping switch: " + identifier);
            await _sceneService.ToggleScene(identifier);
            return Ok("");
        }

        [HttpGet]
        [Route("/api/scenes")]
        public List<Scene> GetScenes()
        {
            return _sceneService.GetAll();

        }

        [HttpPost]
        [Route("/api/scenes")]
        public void SaveSwitches([FromBody] List<Scene> scenes)
        {
            _sceneService.Update(scenes);
        }

        [HttpPost]
        [Route("api/scenes/create")]
        public ActionResult CreateSwitch(Scene scene)
        {
            _sceneService.CreateSwitch(scene);
            return Ok("");
        }

        //[HttpPost]
        //[Route("api/switch/{switchId}/action/create")]
        //public ActionResult CreateSwitchAction(SwitchAction switchAction)
        //{
        //    _switchService.CreateSwitchAction(switchAction);
        //    return Ok("");
        //}
    }
}
