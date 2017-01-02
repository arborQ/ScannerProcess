using Common;
using System.Windows;
using System.Windows.Input;
using WorkflowService;

namespace ScannerReader.Windows
{
    /// <summary>
    /// Interaction logic for WorkflowWindow.xaml
    /// </summary>
    public partial class WorkflowWindow : Window
    {
        private readonly string _userLogin;
        private readonly Workflow Workflow;
        public WorkflowWindow(string userLogin)
        {
            _userLogin = userLogin;

            Workflow = new Workflow
            {
                DisplayMessage = s => {  }
            };

            InitializeComponent();
        }

        private void WorkflowWindow_OnKeyDown(object sender, KeyEventArgs e)
        {
            Workflow.Trigger("lol");
        }
    }
}