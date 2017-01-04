using System;
using WorkflowService.States;

namespace WorkflowService
{
    public class Workflow
    {
        private WorkflowState CurrentState { get; set; }

        public void Start(IWorkflowOutput workflowOutput)
        {
            CurrentState = new PendingWorklowState(workflowOutput);
        }

        public void Trigger(string input)
        {
            if (input == null)
            {
                throw new Exception("Workflow was not started");
            }

            CurrentState = CurrentState.Trigger(input);
        }
    }
}