using System.Windows;
using System.Windows.Threading;
using Logger.Interfaces;
using ScannerReader.Windows;

namespace ScannerReader
{
   
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App
    {
        private readonly ILogService _loggerService;

        public App()
        {
            Bootstrapper.Initialize();

            _loggerService = Bootstrapper.Resolve<ILogService>();
        }

        private void Application_Startup(object sender, StartupEventArgs e)
        {
            _loggerService.ApplicationStart();

#if DEBUG
            var baseWindow = Bootstrapper.Resolve<AdminOptionsWindow>();
#endif

#if !DEBUG
                var baseWindow = Bootstrapper.Resolve<LoginWindow>();
#endif
                baseWindow.Show();

            }

        private void App_OnExit(object sender, ExitEventArgs e)
        {
            _loggerService.ApplicationEnd();
        }

        private void App_OnDispatcherUnhandledException(object sender, DispatcherUnhandledExceptionEventArgs e)
        {
            MessageBox.Show(e.Exception.Message);
            _loggerService.Exception(e.Exception);
            e.Handled = true;
        }
    }


}