using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CramerGui.Services.Interfaces
{
    public interface IInfluxService
    {
        Task<string> GetTemperatureReadings();
        Task<string> GetPowerReadings();
        string GetMonthlyTotals(string fromDate, string toDate);
    }
}
