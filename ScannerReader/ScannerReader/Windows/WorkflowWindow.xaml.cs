using System.Windows;
using System.Windows.Input;
using System.Windows.Threading;
using Common.Interfaces;
using ScannerReader.Models;
using WorkflowService;

namespace ScannerReader.Windows
{
    /// <summary>
    ///     Interaction logic for WorkflowWindow.xaml
    /// </summary>
    public sealed partial class WorkflowWindow
    {
        private readonly IKeyboardReader _keyboardReader;
        private readonly IUserSecurity _userSecurity;
        private readonly Workflow _workflow;

        public WorkflowWindow(IUserSecurity userSecurity, IKeyboardReader keyboardReader, Workflow workflow)
        {
            _userSecurity = userSecurity;
            _keyboardReader = keyboardReader;
            _workflow = workflow;

            InitializeComponent();
        }

        public WorkflowOutput WorkflowOutput { get; set; }

        private void WorkflowWindow_OnKeyDown(object sender, KeyEventArgs e)
        {
            var timeOut = new DispatcherTimer();

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

        private void WorkflowWindow_OnLoaded(object sender, RoutedEventArgs e)
        {
            WorkflowOutput = new WorkflowOutput();

            DataContext = WorkflowOutput;
            _workflow.Start(WorkflowOutput);
        }
    }
}