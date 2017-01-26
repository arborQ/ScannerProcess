using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Input;
using Common.Interfaces;
using ScannerReader.Models;
using WorkflowService;

namespace ScannerReader.Windows
{
    /// <summary>
    /// Interaction logic for WorkflowWindow.xaml
    /// </summary>
    public sealed partial class WorkflowWindow : INotifyPropertyChanged
    {
        private readonly Workflow _workflow;
        private readonly IUserSecurity _userSecurity;
        private readonly IKeyboardReader _keyboardReader;

        public WorkflowOutput WorkflowOutput { get; set; }

        public WorkflowWindow(IUserSecurity userSecurity, IKeyboardReader keyboardReader, Workflow workflow)
        {
            WorkflowOutput = new WorkflowOutput();
            _userSecurity = userSecurity;
            _keyboardReader = keyboardReader;
            _workflow = workflow;

            DataContext = WorkflowOutput;

            InitializeComponent();
        }

        private void WorkflowWindow_OnKeyDown(object sender, KeyEventArgs e)
        {
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
                default:
                    readerResonse = _keyboardReader.NotifyChar(e.Key);
                    break;
            }

            if (!string.IsNullOrEmpty(readerResonse))
            {
                _workflow.Trigger(readerResonse);
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        private void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        private void WorkflowWindow_OnLoaded(object sender, RoutedEventArgs e)
        {
            _workflow.Start(WorkflowOutput);
        }
    }
}