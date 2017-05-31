using EasyModbus;
using Logger.Interfaces;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace ControllerService
{
    internal class ControllerService : IControllerService
    {
        private ControllerEvents _events;
        private string _address;
        private ILogService _logger;

        public ControllerService(ControllerEvents events, ILogService logger, string addressIp)
        {
            _events = events;
            _address = addressIp;
            _logger = logger;
        }

        public async Task<bool> SelectJobAsync(int jobNumber)
        {
            try
            {
                var server = ConnectModbus(_address);
                if (server != null)
                {
                    Stop(server);
                    RunJob(server, SelectedJob(jobNumber - 1));
                    Start(server);
                    await Listen(server, 0);

                    return true;
                }
            }
            catch(Exception e)
            {
                _logger.ControllerAction($"BŁĄD: {e.Message}");
                _logger.Exception(e);

                if (_events.Error != null)
                {
                    _events.Error(e);
                }
            }
            return false;
        }

        private ModbusClient ConnectModbus(string defaultId)
        {
            var port = 502;

            _logger.ControllerAction($"Połącz: {defaultId}:{port}");
            var client = new ModbusClient(defaultId, port);
            client.Connect();
            return client;
        }

        private void RunJob(ModbusClient server, int number)
        {
            _logger.ControllerAction($"Rejestr: 0, Wartość: {number}");
            server.WriteSingleRegister(0, number);
        }

        private void Stop(ModbusClient server)
        {
            _logger.ControllerAction("Rejestr: 0, Wartość: 0");
            server.WriteSingleRegister(0, 0);
            _logger.ControllerAction("Rejestr: 1, Wartość: 0");
            server.WriteSingleRegister(1, 0);
        }

        private void Start(ModbusClient server)
        {
            _logger.ControllerAction("Rejestr: 1, Wartość: 1");
            server.WriteSingleRegister(1, 1);
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
                _logger.ControllerAction($"Czekaj na odpowiedź: {number}:{1}");

                var state = JobType.Done;

                while (state == JobType.Done)
                {
                    var value = server.ReadInputRegisters(number, 1).First();
                    _logger.ControllerAction($"Czekam na {(int)JobType.InProgress} jest {value}");
                    state = (JobType)value;
                    _events.ChangeState("Rozpocznij proszę pracę.");
                    Thread.Sleep(300);
                }

                while (state == JobType.InProgress)
                {
                    var value = server.ReadInputRegisters(number, 1).First();
                    _logger.ControllerAction($"Czekam na {(int)JobType.Done} jest {value}");
                    state = (JobType)value;
                    _events.ChangeState("Praca rozpoczęta...");
                    Thread.Sleep(300);
                }

                _logger.ControllerAction($"KONIEC!");
                if(_events != null && _events.WorkDone != null)
                {
                    _events.WorkDone();
                }
            });
        }
    }
}

