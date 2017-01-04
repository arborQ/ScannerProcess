using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using Common;
using System.Windows;
using System.Windows.Input;
using AdminPanel.Annotations;
using ScannerReader.Models;
using WorkflowService;

namespace ScannerReader.Windows
{
    /// <summary>
    /// Interaction logic for WorkflowWindow.xaml
    /// </summary>
    public partial class WorkflowWindow : INotifyPropertyChanged
    {
        private readonly string _userLogin;
        private readonly Workflow Workflow;
        private string _stepDescription;

        public WorkflowOutput WorkflowOutput { get; set; }

#if DEBUG
        public WorkflowWindow()
            :this("no-context")
        {
            
        }
#endif
        public WorkflowWindow(string userLogin)
        {
            WorkflowOutput = new WorkflowOutput();
            _userLogin = userLogin;

            DataContext = WorkflowOutput;

            Workflow = new Workflow(WorkflowOutput);

            InitializeComponent();
            Workflow.Start();
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
            Workflow.Trigger("lol");
        }

        public event PropertyChangedEventHandler PropertyChanged;

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}