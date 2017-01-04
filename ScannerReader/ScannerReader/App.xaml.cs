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
#if DEBUG
            var baseWindow = Bootstrapper.Resolve<WorkflowWindow>();
            baseWindow.ShowDialog();
#endif

            base.OnStartup(e);
        }
    }
}