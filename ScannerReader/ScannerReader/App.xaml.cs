using System.Windows;
using ScannerReader.Windows;

namespace ScannerReader
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App
    {
        public App()
        {
            Bootstrapper.Initialize();
        }

        private void Application_Startup(object sender, StartupEventArgs e)
        {
#if DEBUG
            var baseWindow = Bootstrapper.Resolve<WorkflowWindow>();
#endif

#if !DEBUG
                var baseWindow = Bootstrapper.Resolve<LoginWindow>();
#endif
                baseWindow.Show();

            }
        }
}