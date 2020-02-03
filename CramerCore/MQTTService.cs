using Microsoft.Extensions.Hosting;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mqtt;
using System.Threading;
using System.Threading.Tasks;

namespace CramerCore
{
    internal class MqttCredentials : MqttClientCredentials
    {
        public MqttCredentials(string clientId) : base(clientId)
        {
        }
    }
    public enum LightMode
    {
        on = 1,
        off = 0
    }
    public class MqttMessage
    {
        public int? Percentage { get; set; }
        public LightMode? Mode { get; set; }
    }

    public class MqttService : IHostedService
    {
        private SerialService _serialService;

        public MqttService(SerialService serialService)
        {
            _serialService = serialService;
        }

        public async Task StartAsync(CancellationToken cancellationToken)
        {
            Console.WriteLine("Starting MQTT service");
            var environment = Environment.GetEnvironmentVariables();
            string mqttIp = (string)environment["MqttIp"];
            int.TryParse((string)environment["MqttPort"], out int mqttPort);

            await StartMqtt(mqttIp, mqttPort);
        }

        public async Task StartMqtt(string mqttIp, int mqttPort)
        {
            IMqttClient mqttClient = await MqttClient.CreateAsync(mqttIp, mqttPort);
            mqttClient.Disconnected += async (a, b) => await ConnectToMqtt(mqttClient);
            await ConnectToMqtt(mqttClient);

            mqttClient.MessageStream.Subscribe(msg =>
            {
                HandleMessage(msg).Wait();
            });

        }

        private async Task ConnectToMqtt(IMqttClient mqttClient)
        {
            Random random = new Random();
            await mqttClient.ConnectAsync(new MqttCredentials(clientId: "Mqtt2Arduino" + random.Next(1000)));
            await mqttClient.SubscribeAsync("cramer62/lights/core/#", MqttQualityOfService.AtMostOnce);
            Console.WriteLine("Connected to Mqtt");
        }

        private async Task HandleMessage(MqttApplicationMessage message)
        {
            try
            {
                string name = message.Topic.Split('/').Last();
                Console.WriteLine($"recieved {name}, {System.Text.Encoding.Default.GetString(message.Payload)}");

                MqttMessage payload = JsonConvert.DeserializeObject<MqttMessage>(System.Text.Encoding.Default.GetString(message.Payload));

                string type = message.Topic.Split('/')[1];

                int lightId;
                if (type != "lights" || !int.TryParse(name, out lightId))
                {
                    return;
                }

                if (payload.Mode != null)
                {
                    if (payload.Mode == LightMode.on)
                    {
                        await _serialService.SendData($"l{ lightId.ToString("00") }1");
                    }
                    else
                    {
                        await _serialService.SendData($"l{ lightId.ToString("00") }0");
                    }
                }

                if (payload.Percentage != null)
                {
                    long newValue = ((payload.Percentage.Value) * 255) / 100;

                    await _serialService.SendData($"d{ lightId.ToString("00") }{ newValue.ToString("000") }");
                }

            }
            catch (Exception e)
            {
                Console.WriteLine("Mqtt exception: " + e.Message);
            }
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            Console.WriteLine("StopAsync");
            return Task.CompletedTask;
        }
    }
}
