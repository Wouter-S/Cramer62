using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace CramerCore
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = new HostBuilder()
                .ConfigureServices((services) =>
                {
                    
                    services.AddSingleton<SerialService>();
                    services.AddHostedService<MqttService>();
                });

            await builder.RunConsoleAsync();
        }
    }
}
