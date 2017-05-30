using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;

namespace ScannerReader.Windows
{
    /// <summary>
    ///     Interaction logic for WorkInProgress.xaml
    /// </summary>
    public partial class WorkInProgress
    {
        private readonly IList<Func<string>> _listOfActions;
        private bool _abort = false;
        public WorkInProgress(IEnumerable<Func<string>> listOfActions)
        {
            _listOfActions = listOfActions.ToList();
            InitializeComponent();
        }

        private async void WorkInProgress_OnLoaded(object sender, RoutedEventArgs e)
        {
            var screenWidth = SystemParameters.PrimaryScreenWidth;
            var screenHeight = SystemParameters.PrimaryScreenHeight;
            var windowWidth = Width;
            var windowHeight = Height;
            Left = screenWidth / 2 - windowWidth / 2;
            Top = screenHeight / 2 - windowHeight / 2;

            await Task.Run(async () =>
            {
                await ProgressBar.Dispatcher.InvokeAsync(() => { ProgressBar.Maximum = _listOfActions?.Count ?? 0; });

                foreach (var listOfAction in _listOfActions)
                {
                    if (_abort)
                    {
                        break;
                    }

                    await Task.Factory.StartNew(async () =>
                    {
                        var message = listOfAction();
                        await ProgressBar.Dispatcher.InvokeAsync(() => { ProgressBar.Value += 1; });

                        await ProgressMessage.Dispatcher.InvokeAsync(() => { ProgressMessage.Text = message; });
                    });
                }
            });
            Close();
        }


        protected override void OnClosing(CancelEventArgs e)
        {
            _abort = true;
            base.OnClosing(e);
        }
    }
}