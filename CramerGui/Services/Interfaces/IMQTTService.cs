using System.Threading.Tasks;

namespace CramerGui.Services
{
    public interface IMqttService
    {
        Task StartMqtt();
    }
}