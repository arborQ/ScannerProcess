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
    }
}