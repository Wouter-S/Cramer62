using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Mqtt;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using CramerAlexa.Services.Interfaces;

namespace CramerAlexa.Services
{
    public class UdpStateInfo
    {
        public UdpStateInfo(UdpClient c, IPEndPoint ep)
        {
            client = c;
            endpoint = ep;
        }
        public UdpClient client;
        public IPEndPoint endpoint;
    }

    public class HueService : IHueService
    {
        private IMqttClient mqttClient;
        private HttpClient httpClient;

        private Mqtt _mqttSettings;
        private Global _globalSettings;

        private static bool running;
        private static string MulticastIP;
        private static string MulticastLocalIP;
        private static int MulticastPort;
        public static string UUID;
        public static int WebServerPort;

        private static UdpClient MulticastClient;

        private static byte[] byteDiscovery;
        public static string DiscoveryResponse;
        private static string discoveryTemplate = "HTTP/1.1 200 OK\r\n" +
            "CACHE-CONTROL: max-age=86400\r\n" +
            "EXT:\r\n" +
            "LOCATION: http://{0}:{1}/api/setup.xml\r\n" +
            "OPT: \"http://schemas.upnp.org/upnp/1/0/\"; ns=01\r\n" +
            "01-NLS: {2}\r\n" +
            "ST: urn:schemas-upnp-org:device:basic:1\r\n" +
            "USN: uuid:Socket-1_0-221438K0100073::urn:Belkin:device:**\r\n\r\n";

        public HueService(IOptions<Global> globalSettings, IOptions<Mqtt> options)
        {
            _mqttSettings = options.Value;
            _globalSettings = globalSettings.Value;
            string ipAddress = globalSettings.Value.IpAddress;
            int port = globalSettings.Value.Port;
            Console.WriteLine("starting in ip: " + ipAddress);

            MulticastIP = "239.255.255.250";
            MulticastPort = 1900;
            MulticastLocalIP = ipAddress;
            WebServerPort = port;
            UUID = "aef85303-330a-4eab-b28d-038ac90416ab";
            DiscoveryResponse = string.Format(discoveryTemplate, MulticastLocalIP, WebServerPort, UUID);
            byteDiscovery = Encoding.ASCII.GetBytes(DiscoveryResponse);
            running = false;

            MulticastClient = new UdpClient
            {
                ExclusiveAddressUse = false
            };

            var ipEndpoint = new IPEndPoint(0, MulticastPort);

            //patch for Issue #12 from gibman to allow non-exclusive use of the port
            MulticastClient.Client.SetSocketOption(SocketOptionLevel.Socket, SocketOptionName.ExclusiveAddressUse, false);
            MulticastClient.Client.SetSocketOption(SocketOptionLevel.Socket, SocketOptionName.ReuseAddress, true);

            MulticastClient.Client.Bind(ipEndpoint);

            IPAddress ipSSDP = IPAddress.Parse(MulticastIP);

            MulticastClient.JoinMulticastGroup(ipSSDP, IPAddress.Parse(MulticastLocalIP));

            running = true;

            UdpStateInfo udpListener = new UdpStateInfo(MulticastClient, new IPEndPoint(ipSSDP, MulticastPort));

            MulticastClient.BeginReceive(new AsyncCallback(MulticastReceiveCallback), udpListener);
        }

        private static bool IsSSDPDiscoveryPacket(string message)
        {
            if (message != null && message.ToUpper().StartsWith("M-SEARCH * HTTP/1.1") && message.ToLower().Contains("ssdp:discover"))
            {
                return true;
            }
            return false;
        }

        public static void MulticastReceiveCallback(IAsyncResult ar)
        {
            try
            {
                UdpStateInfo udpListener = (UdpStateInfo)(ar.AsyncState);
                UdpClient client = udpListener.client;
                IPEndPoint endpoint = udpListener.endpoint;

                if (client != null)
                {
                    Byte[] receiveBytes = client.EndReceive(ar, ref endpoint);
                    string receiveString = Encoding.ASCII.GetString(receiveBytes);

                    //discovery has occured, send our response
                    if (IsSSDPDiscoveryPacket(receiveString))
                    {
                        Socket WinSocket = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);

                        WinSocket.Connect(endpoint);
                        WinSocket.Send(byteDiscovery);
                        WinSocket.Shutdown(SocketShutdown.Both);
                        WinSocket.Close();
                    }

                }
                if (running)
                {
                    MulticastClient.BeginReceive(new AsyncCallback(MulticastReceiveCallback), udpListener);
                }

            }
            catch (Exception ex)
            {
                Console.WriteLine("Error occured in MulticastReceiveCallBack.", ex);
            }
        }

        public async Task Init()
        {
            var configuration = new MqttConfiguration { Port = _mqttSettings.Port };
            mqttClient = await MqttClient.CreateAsync(_mqttSettings.IpAddress, configuration);
            
            mqttClient.Disconnected += async (a, b) => await ConnectToMqtt();
            await ConnectToMqtt();
            httpClient = new HttpClient();
        }
        private async Task ConnectToMqtt()
        {
            await mqttClient.ConnectAsync();
            Console.WriteLine("Connected to Mqtt");
        }

        public async Task DimLight(string lightId, int percentage, bool v3)
        {
            Light light = await GetLight(lightId);

            string content = "{ \"percentage\": \"" + percentage.ToString() + "\" }";
            var message = new MqttApplicationMessage(light.MqttAddress, Encoding.UTF8.GetBytes(content));
            await mqttClient.PublishAsync(message, MqttQualityOfService.AtMostOnce);
        }

        public async Task SwitchLight(string lightId, LightMode lightMode, bool v2)
        {
            Light light = await GetLight(lightId);
            string content = "{ \"mode\": \"" + lightMode.ToString() + "\" }";
            var message = new MqttApplicationMessage(light.MqttAddress, Encoding.UTF8.GetBytes(content));
            await mqttClient.PublishAsync(message, MqttQualityOfService.AtMostOnce);
        }

        public async Task<Light> GetLight(string lightId)
        {
            var response = await httpClient.GetAsync($"http://{_globalSettings.LightApiBaseUrl}/api/lights/{lightId}");
            Light light = JsonConvert.DeserializeObject<Light>(await response.Content.ReadAsStringAsync());
            return light;
        }

        public async Task<List<Light>> GetLights()
        {
            var response = await httpClient.GetAsync($"http://{_globalSettings.LightApiBaseUrl}/api/lights");
            List<Light> lights = JsonConvert.DeserializeObject<List<Light>>(await response.Content.ReadAsStringAsync());
            Console.WriteLine($"got {lights.Count} lights");
            return lights;
        }
    }
}
