using System;
using System.ComponentModel;
using System.Timers;
using System.Windows;
using System.Windows.Input;
using Castle.Core.Logging;
using Common.Interfaces;
using Logger.Interfaces;
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
        private readonly ILogService _logger;
        private readonly IKeyboardReader _keyboardReader;
        private readonly IUserSecurity _userSecurity;
        private readonly Workflow _workflow;

        private Timer _timer;

        public WorkflowWindow(IUserSecurity userSecurity, IKeyboardReader keyboardReader, Workflow workflow,
            ApplicationService applicationService, ILogService logger)
        {
            _userSecurity = userSecurity;
            _keyboardReader = keyboardReader;
            _workflow = workflow;
            _applicationService = applicationService;
            _logger = logger;

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
                    readerResonse = "G01880360      ";
                    break;
                case Key.F2:
                    readerResonse = "13304R04F#G01880360";
                    break;
                case Key.F3:
                    readerResonse = "13304R04F#G01880361";
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

            if (!string.IsNullOrEmpty(readerResonse) && !_workflow.IsLocked)
            {
                _logger.ScanCode(readerResonse);
                WorkflowOutput.Actions.Insert(0, readerResonse);
                _workflow.Trigger(readerResonse);
            }
        }

        private async void WorkflowWindow_OnLoaded(object sender, RoutedEventArgs e)
        {
            CloseReason = string.Empty;

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
            _keyboardReader.Dispose();

            base.OnClosing(e);
        }
    }
}