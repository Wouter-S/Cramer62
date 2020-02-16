using CramerGui.Hubs;
using CramerGui.Repositories;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mqtt;
using System.Text;
using System.Threading.Tasks;

namespace CramerGui.Services
{
    public class LightService : ILightService
    {
        private IRoomRepository _roomRepo;
        private ILightRepository _lightRepo;
        private IHubContext<TheHub> _rulesHubContext;
        private IMqttClient client;
        private Mqtt _mqttSettings;

        public LightService(IOptions<Mqtt> options, IRoomRepository roomRepository, ILightRepository lightRepository, IHubContext<TheHub> hubContext)
        {
            _roomRepo = roomRepository;
            _lightRepo = lightRepository;
            _rulesHubContext = hubContext;
            _mqttSettings = options.Value;
        }

        public async Task Init()
        {
            var configuration = new MqttConfiguration { Port = _mqttSettings.Port };
            client = await MqttClient.CreateAsync(_mqttSettings.IpAddress, configuration);
            await client.ConnectAsync();

            client.Disconnected += async (a, b) => await client.ConnectAsync();
        }

        public Light GetLight(long lightId)
        {
            return _lightRepo.GetLight(lightId);
        }

        public Light GetLight(string friendlyName)
        {
            return _lightRepo.GetLight(friendlyName);
        }


        internal List<Light> GetAll()
        {
            return _lightRepo.GetLights();
        }

        internal void Update(Light light)
        {
            _lightRepo.Update(light);
        }

        public List<Light> GetLight()
        {
            return _lightRepo.GetLights();
        }

        public List<Light> GetLightsByRoom(long roomId)
        {
            return _lightRepo.GetLightsByRoom(roomId);
        }

        public async Task SwitchLight(long lightId, LightMode mode, bool alert = false)
        {
            Light light = _lightRepo.GetLight(lightId);

            if (mode == LightMode.on)
            {
                if (light.IsDimmable)
                {
                    await DimLight(new long[] { light.LightId }, light.Percentage.Value, mode);
                }
                else
                {
                    _lightRepo.SetLights(lightId, mode);
                    string content = "{ \"mode\": \"on\" }";
                    var message = new MqttApplicationMessage(light.MqttAddress, Encoding.UTF8.GetBytes(content));
                    await PublishMqttMessage(message);

                }
            }
            else
            {
                if (light.IsDimmable)
                {

                    await DimLight(new long[] { light.LightId }, light.Percentage.Value, mode);
                }
                else
                {
                    _lightRepo.SetLights(lightId, mode);

                    string content = "{ \"mode\": \"off\" }";
                    var message = new MqttApplicationMessage(light.MqttAddress, Encoding.UTF8.GetBytes(content));
                    await PublishMqttMessage(message);
                }
            }

            _lightRepo.SetLightLog(lightId, (long)mode);

            if (alert)
            {
                await LightsChanged();
            }
        }

        public async Task DimLight(long lightId, long percentage, bool alert = false)
        {
            await DimLight(new long[] { lightId }, percentage, alert);
        }
        public async Task DimLight(long[] lightIds, long percentage, bool alert = false)
        {
            await DimLight(lightIds, percentage, null, alert);
        }
        public async Task DimLight(long[] lightIds, long percentage, LightMode? lightMode, bool alert = false)
        {
            List<Light> lights = new List<Light>();
            foreach (long lightId in lightIds)
            {
                Light light = _lightRepo.GetLight(lightId);
                if (!light.IsDimmable)
                {
                    continue;
                }
                lights.Add(light);

                if (percentage == 0 || lightMode == LightMode.off)
                {
                    _lightRepo.SetLights(lightId, LightMode.off);
                }
                else if (lightMode != LightMode.off)
                {
                    _lightRepo.SetLights(lightId, LightMode.on);
                    _lightRepo.SetLights(lightId, percentage);
                }
                _lightRepo.SetLightLog(lightId, (lightMode == null || lightMode == LightMode.on) ? percentage : 0);
            }

            if (lightMode == LightMode.off)
            {
                percentage = 0;
            }

            foreach (Light light in lights)
            {
                string content = "{ \"percentage\": " + percentage + " }";
                var message = new MqttApplicationMessage(light.MqttAddress, Encoding.UTF8.GetBytes(content));
                await PublishMqttMessage(message);
            }

            if (alert)
            {
                await LightsChanged();
            }
        }

        public async Task SwitchRoomLight(long roomId, LightMode mode, bool isAutomatic = false)
        {
            foreach (Light light in _roomRepo.GetRoom(roomId).RoomLights)
            {
                await SwitchLight(light.LightId, mode);
            }
            await LightsChanged();
        }

        public async Task DimRoomLight(long roomId, long percentage)
        {
            await DimLight(this.GetLightsByRoom(roomId).Select(s => s.LightId).ToArray(), percentage);
        }

        public async Task KillLights()
        {
            List<Light> lights = _lightRepo.GetLights().Where(l => l.RoomId.HasValue).ToList();
            foreach (Light light in lights.OrderByDescending(l => l.Mode))
            {
                await this.SwitchLight(light.LightId, LightMode.off);
                //await Task.Delay(250);
            }

            await LightsChanged();
        }

        public async Task LightsChanged()
        {
            await _rulesHubContext.Clients.All.SendAsync("LightsChanged");
        }

        public List<LightLog> GetLightLogs()
        {
            List<LightLog> logs = _lightRepo.GetLightLogs();
            List<Light> lights = _lightRepo.GetLights();
            foreach (LightLog log in logs)
            {
                log.LightName = lights.Single(s => s.LightId == log.LightId).Name;
            }
            return logs;
        }

        public Task UpdateLights(List<Light> lights)
        {
            lights.ForEach(l => _lightRepo.Update(l));
            return Task.FromResult(0);
        }

        private async Task PublishMqttMessage(MqttApplicationMessage message)
        {
            try
            {
                await client.PublishAsync(message, MqttQualityOfService.AtMostOnce);
                Console.WriteLine($"PublishAsync message: {message.Payload} on topic {message.Topic}");

            }
            catch (Exception e)
            {
                Console.WriteLine($"PublishAsync crash { e.Message }; connected: {client.IsConnected}");
            }
        }
    }
}
