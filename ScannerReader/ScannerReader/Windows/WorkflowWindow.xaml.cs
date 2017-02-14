using System;
using System.ComponentModel;
using System.Timers;
using System.Windows;
using System.Windows.Input;
using Common.Interfaces;
using RepositoryServices;
using ScannerReader.Models;
using WorkflowService;

namespace ScannerReader.Windows
{
    /// <summary>
    ///     Interaction logic for WorkflowWindow.xaml
    /// </summary>
    public sealed partial class WorkflowWindow
    {
        private readonly ApplicationService _applicationService;
        private readonly IKeyboardReader _keyboardReader;
        private readonly IUserSecurity _userSecurity;
        private readonly Workflow _workflow;

        private Timer _timer;

        public WorkflowWindow(IUserSecurity userSecurity, IKeyboardReader keyboardReader, Workflow workflow,
            ApplicationService applicationService)
        {
            _userSecurity = userSecurity;
            _keyboardReader = keyboardReader;
            _workflow = workflow;
            _applicationService = applicationService;

            InitializeComponent();
        }

        public string CloseReason { get; private set; }

        public WorkflowOutput WorkflowOutput { get; set; }

        private void WorkflowWindow_OnKeyDown(object sender, KeyEventArgs e)
        {
            _timer.Stop();
            _timer.Start();

            var readerResonse = string.Empty;
            switch (e.Key)
            {
#if DEBUG
                case Key.F1:
                    readerResonse = "20160905-00165#53A08VN0H";
                    break;
                case Key.F2:
                    readerResonse = "20160905-00165#53A08VN0C";
                    break;
                case Key.F3:
                    readerResonse = "20160905-00165#53A08VN0D";
                    break;
#endif
                case Key.Escape:
                    if (_workflow.CanBreak)
                        Close();
                    return;
                default:
                    readerResonse = _keyboardReader.NotifyChar(e.Key);
                    break;
            }

            if (!string.IsNullOrEmpty(readerResonse))
                _workflow.Trigger(readerResonse);
        }

        private async void WorkflowWindow_OnLoaded(object sender, RoutedEventArgs e)
        {
            WorkflowOutput = new WorkflowOutput();
            var timeOut = _applicationService.SettingsRepository.Get().DefaultTimeout;

            DataContext = WorkflowOutput;
            _workflow.Start(WorkflowOutput);

            _timer = new Timer(TimeSpan.FromMinutes(timeOut).TotalMilliseconds);

            _timer.Elapsed += async (o, args) =>
            {
                await Dispatcher.InvokeAsync(() =>
                {
                    if (_workflow.CanBreak)
                    {
                        CloseReason = "Timeout";
                        Close();
                    }
                });
            };
            _timer.Start();
        }

        protected override void OnClosing(CancelEventArgs e)
        {
            _timer.Dispose();
            base.OnClosing(e);
        }
    }
}