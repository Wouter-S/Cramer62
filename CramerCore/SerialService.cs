using RJCP.IO.Ports;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;


namespace CramerCore
{
    public class SerialService
    {
        private SerialPortStream _writerPort;
        private static SemaphoreSlim semaphoreSlim = new SemaphoreSlim(1, 1);

        public SerialService()
        {
            var environment = Environment.GetEnvironmentVariables();
            string comPort = (string)environment["ComPort"];

            Console.WriteLine("Starting SerialService");

            try
            {
                _writerPort = new SerialPortStream(comPort)
                {
                    BaudRate = 1000000,
                    Parity = Parity.None,
                    StopBits = StopBits.One,
                    DataBits = 8,
                    Handshake = Handshake.None,
                    DtrEnable = true,
                    RtsEnable = true,
                    Encoding = System.Text.Encoding.ASCII,
                };

                if (!_writerPort.IsOpen)
                {
                    _writerPort.Open();
                    Console.WriteLine("port open");
                }

            }
            catch (Exception e)
            {
                Console.WriteLine("crash: " + e.Message);
            }
        }

        public async Task SendData(string text)
        {
            try
            {
                await semaphoreSlim.WaitAsync();
                await _writerPort.WriteAsync(Encoding.UTF8.GetBytes(text));
                await Task.Delay(250);
                Console.WriteLine("Serial sent: " + text);
            }
            catch (Exception e)
            {
                Console.WriteLine("Listener write crash: " + e.Message);
            }
            finally
            {
                semaphoreSlim.Release();
            }
        }
    }
}