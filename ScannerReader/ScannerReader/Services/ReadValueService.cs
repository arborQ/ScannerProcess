using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using ScannerReader.Interfaces;
using WorkflowService.Interfaces;

namespace ScannerReader.Services
{
    internal class ReadValueService : IReadValueService
    {
        private readonly IWindowFactory _windowFactory;
        public ReadValueService(IWindowFactory windowFactory)
        {
            _windowFactory = windowFactory;
        }

        public bool ReadValue(int expected)
        {
            var window = _windowFactory.CreateGetValueWindow(expected.ToString());
            window.ShowDialog();

            return window.Model.Value == expected.ToString();
        }

        public void Wait(TimeSpan time)
        {
            var seconds = (int)time.TotalSeconds;

            var actions = Enumerable.Range(0, seconds)
                .OrderByDescending(s => s)
                .Select(a => new Func<string>(() =>
                {
                    Thread.Sleep(TimeSpan.FromSeconds(1));
                    return $"Pracuje: {a}s";
                }))
                .ToList();

            var waitWindow = _windowFactory.WorkInProgress(actions);
            waitWindow.ShowDialog();
        }
    }
}