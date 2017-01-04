using WorkflowService.States;

namespace WorkflowService
{
    public class Workflow
    {
        private readonly IWorkflowOutput _workflowOutput;

        public Workflow(IWorkflowOutput workflowOutput)
        {
            _workflowOutput = workflowOutput;
        }

        private WorkflowState CurrentState { get; set; }

        public void Start()
        {
            CurrentState = new PendingWorklowState(_workflowOutput);
        }

        public void Trigger(string input)
        {
            CurrentState = CurrentState.Trigger(input);
        }
    }
}