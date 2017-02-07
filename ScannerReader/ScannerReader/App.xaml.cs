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


            base.OnStartup(e);
        }

        private void Application_Startup(object sender, StartupEventArgs e)
        {
#if DEBUG
            var baseWindow = Bootstrapper.Resolve<AdminOptionsWindow>();
#endif

#if !DEBUG
                var baseWindow = Bootstrapper.Resolve<LoginWindow>();
#endif
                baseWindow.Show();

            }
        }
}