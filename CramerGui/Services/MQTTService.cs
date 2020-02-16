using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mqtt;
using System.Threading.Tasks;

namespace CramerGui.Services
{
    internal class MqttCredentials : MqttClientCredentials
    {
        public MqttCredentials(string clientId) : base(clientId)
        {
        }
    }

    public class MqttMessage
    {
        [JsonProperty("mode")]
        public LightMode? Mode { get; internal set; }

        [JsonProperty("percentage")]
        public int? Percentage { get; internal set; }
    }

    public class MqttService : IMqttService
    {
        private IOptions<Mqtt> _mqttSettings;
        private readonly ILightService _lightService;
        private IMqttClient _mqttClient;

        public MqttService(ILightService lightService, IOptions<Mqtt> mqttSettings)
        {
            _mqttSettings = mqttSettings;
            _lightService = lightService;
        }

        private async Task Connect()
        {
            Random random = new Random();
            string clientId = $"Gui{random.Next(1000)}";

            Console.WriteLine($"Mqtt: Attempting to connect with clientId {clientId}");

            var sessionState = await _mqttClient.ConnectAsync(new MqttCredentials(clientId));
            await _mqttClient.SubscribeAsync("cramer62/lights/+", MqttQualityOfService.AtMostOnce);
            _mqttClient.MessageStream.Subscribe(msg => HandleMessage(msg).Wait());
            //_mqttClient.MessageStream.Subscribe(async msg => await HandleMessage(msg));
            Console.WriteLine($"started MQTT: {JsonConvert.SerializeObject(_mqttSettings.Value)} - {sessionState.ToString()}");
        }

        public async Task StartMqtt()
        {

            try
            {
                _mqttClient = await MqttClient.CreateAsync(_mqttSettings.Value.IpAddress, _mqttSettings.Value.Port);
                _mqttClient.Disconnected += async (a, b) => await Client_Disconnected(a, b);

                await Connect();
            }
            catch (Exception e)
            {
                Console.WriteLine("Mqtt ex: " + e.Message);
            }
        }

        private async Task Client_Disconnected(object sender, MqttEndpointDisconnected e)
        {
            Console.WriteLine($"MQTT, disconnected {e.Message} {e.Reason}");
            try
            {
                await Connect();
            }
            catch (Exception)
            {
            }

        }

        private async Task HandleMessage(MqttApplicationMessage message)
        {
            try
            {
                Console.WriteLine($"MQTT, got message on topic {message.Topic}");

                string name = message.Topic.Split('/').Last();
                MqttMessage payload = null;
                if (message.Payload == null)
                {
                    payload = new MqttMessage();
                }
                else
                {
                    payload = JsonConvert.DeserializeObject<MqttMessage>(System.Text.Encoding.Default.GetString(message.Payload));
                }
                
                Console.WriteLine($"MQTT payload: {JsonConvert.SerializeObject(payload)}");

                string type = message.Topic.Split('/')[1];

                switch (type)
                {
                    case "lights":
                        if (int.TryParse(name, out _))
                        {
                            break;
                        }
                        if (name.ToLower() == "killall")
                        {
                            await _lightService.KillLights();
                            break;
                        }

                        Light light = _lightService.GetLight(name);
                        if (light == null)
                        {
                            return;
                        }
                        LightMode lightMode;

                        if (payload.Mode == null && payload.Percentage == null)
                        {
                            lightMode = light.Mode == LightMode.on ? LightMode.off : LightMode.on;
                            await _lightService.SwitchLight(light.LightId, lightMode, true);
                        }
                        else if (payload.Mode != null)
                        {
                            lightMode = payload.Mode.Value;
                            await _lightService.SwitchLight(light.LightId, lightMode, true);
                        }
                        else if (payload.Percentage != null)
                        {
                            await _lightService.DimLight(light.LightId, payload.Percentage.Value, true);
                        }

                        break;
                    default:
                        break;
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("Mqtt exception : " + e.Message);
            }
        }
    }
}
