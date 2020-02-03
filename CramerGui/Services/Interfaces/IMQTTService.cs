using System.Threading.Tasks;

namespace CramerAlexa.Services
{
    public interface IMqttService
    {
        Task StartMqtt();
    }
}