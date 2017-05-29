using EasyModbus;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ControllerService
{
    internal class ControllerService : IControllerService
    {
        private ControllerEvents _events;
        private string _address;
        public ControllerService(ControllerEvents events, string addressIp)
        {
            _events = events;
            _address = addressIp;
        }

        public async Task<bool> SelectJobAsync(int jobNumber)
        {
            try
            {
                var server = await ConnectModbus(_address);
                if (server != null)
                {
                    await Stop(server);
                    await RunJob(server, SelectedJob(jobNumber - 1));
                    await Start(server);
                    await Listen(server, 0);

                    return true;
                }
            }
            catch
            {
            }
            return false;
        }

        private async Task<ModbusClient> ConnectModbus(string defaultId)
        {
            return await Task.Factory.StartNew(() =>
             {
                 try
                 {

                     var port = 502;
                     var client = new ModbusClient(defaultId, port);
                     client.Connect();
                     if (!client.Available(100))
                     {
                         _events.Error($"Nie mogę połączyć: {defaultId}:{port}");
                         return null;
                     }
                     return client;
                 }
                 catch (Exception ex)
                 {
                     if (_events.Error != null)
                     {
                         _events.Error(ex.Message);
                     }
                 }

                 return null;
             });

        }

        private async Task RunJob(ModbusClient server, int number)
        {
            await Task.Factory.StartNew(() =>
            {
                server.WriteSingleRegister(0, number);
            });
        }

        private async Task Stop(ModbusClient server)
        {
            await Task.Factory.StartNew(() =>
            {
                server.WriteSingleRegister(2, 1);
                server.WriteSingleRegister(0, 0);
                server.WriteSingleRegister(1, 0);
            });
        }

        private async Task Start(ModbusClient server)
        {
            await Task.Factory.StartNew(() =>
            {
                server.WriteSingleRegister(1, 1);
            });
        }

        private int SelectedJob(int number)
        {
            var chars = Enumerable.Range(0, 8).Select((i, c) => i == number ? '1' : '0').Reverse();
            var result = Convert.ToInt32(string.Join("", chars), 2);
            return result;
        }

        enum JobType
        {
            InProgress = 0,
            Done = 1
        }

        async Task Listen(ModbusClient server, int number)
        {
            await Task.Factory.StartNew(() =>
            {
                var state = JobType.Done;

                while (state == JobType.Done)
                {
                    state = (JobType)server.ReadInputRegisters(number, 1).First();
                    _events.ChangeState("Rozpocznij proszę pracę.");
                    Thread.Sleep(300);
                }

                while (state == JobType.InProgress)
                {
                    state = (JobType)server.ReadInputRegisters(number, 1).First();
                    Console.Clear();
                    _events.ChangeState("Praca rozpoczęta...");
                    Thread.Sleep(300);
                }

                _events.WorkDone();
            });
        }
    }
}
