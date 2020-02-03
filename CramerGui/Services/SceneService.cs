using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mqtt;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using CramerAlexa.Repositories;

namespace CramerAlexa.Services
{
    public class SceneService : ISceneService
    {

        private IRoomService _roomService;
        private ILightService _lightService;
        private ISceneRepository _sceneRepo;
        private IMqttClient client;
        private Mqtt _mqttSettings;

        public SceneService(IOptions<Mqtt> options, IRoomService roomService, ILightService lightService, ISceneRepository switchRepo)
        {
            _roomService = roomService;
            _lightService = lightService;
            _sceneRepo = switchRepo;
            _mqttSettings = options.Value;
        }

        public async Task Init()
        {
            var configuration = new MqttConfiguration { Port = _mqttSettings.Port };
            client = await MqttClient.CreateAsync(_mqttSettings.IpAddress, configuration);
            client.Disconnected += async (a, b) => await client.ConnectAsync();
            await client.ConnectAsync();
        }

        public void CreateSwitch(Scene scene)
        {
            _sceneRepo.CreateScene(scene);
        }

        public List<Scene> GetAll()
        {
            return _sceneRepo.GetAll();
        }
        public void Update(List<Scene> scenes)
        {
            List<Scene> dbScenes = _sceneRepo.GetAll();
            List<Scene> scenesToDelete = dbScenes.Where(d => !(scenes.Select(s => s.SceneId).Contains(d.SceneId))).ToList();
            scenesToDelete.ForEach(sceneToDelete =>
                {
                    _sceneRepo.DeleteScene(sceneToDelete);
                });

            scenes.ForEach(s =>
            {
                if (s.SceneId == 0)
                {
                    _sceneRepo.CreateScene(s);

                }
                else
                {
                    _sceneRepo.Update(s);
                }
            });
        }

        public async Task ToggleScene(string identifier)
        {
            Scene scene = _sceneRepo.GetScene(long.Parse(identifier));

            var message = new MqttApplicationMessage(scene.MqttAddress, Encoding.UTF8.GetBytes(""));
            try
            {
                await client.PublishAsync(message, MqttQualityOfService.AtMostOnce);
            }
            catch (Exception e)
            {
                Console.WriteLine($"PublishAsync crash { e.Message }");
            }
        }
    }
}
