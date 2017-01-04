using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Input;
using AdminPanel.Annotations;
using Common.Interfaces;
using ScannerReader.Models;
using WorkflowService;

namespace ScannerReader.Windows
{
    /// <summary>
    /// Interaction logic for WorkflowWindow.xaml
    /// </summary>
    public partial class WorkflowWindow : INotifyPropertyChanged
    {
        private readonly Workflow _workflow;
        private readonly IUserSecurity _userSecurity;
        private readonly IKeyboardReader _keyboardReader;
        private string _stepDescription;

        public WorkflowOutput WorkflowOutput { get; set; }

//#if DEBUG
//        public WorkflowWindow(IKeyboardReader keyboardReader, Workflow workflow)
//            :this("no-context", keyboardReader, workflow)
//        {
//        }
//#endif
        public WorkflowWindow(IUserSecurity userSecurity, IKeyboardReader keyboardReader, Workflow workflow)
        {
            WorkflowOutput = new WorkflowOutput();
            _userSecurity = userSecurity;
            _keyboardReader = keyboardReader;
            _workflow = workflow;

            DataContext = WorkflowOutput;

            InitializeComponent();
        }

        public string StepDescription
        {
            get { return _stepDescription; }
            set
            {
                _stepDescription = value;
                OnPropertyChanged();
            }
        }

        private void WorkflowWindow_OnKeyDown(object sender, KeyEventArgs e)
        {
            var readerResonse = _keyboardReader.NotifyChar(e.Key);
            if (!string.IsNullOrEmpty(readerResonse))
            {
                _workflow.Trigger(readerResonse);
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        private void WorkflowWindow_OnLoaded(object sender, RoutedEventArgs e)
        {
            _workflow.Start(WorkflowOutput);
        }
    }
}