using System.Windows;
using ScannerReader.Windows;

namespace ScannerReader
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        public App()
        {
            Bootstrapper.Initialize();
        }

        protected override void OnStartup(StartupEventArgs e)
        {
            var baseWindow = Bootstrapper.Container.GetInstance<WorkflowWindow>();
            baseWindow.ShowDialog();
        }
    }
}