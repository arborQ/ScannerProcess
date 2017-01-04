using System;
using WorkflowService.States;

namespace WorkflowService
{
    public class Workflow
    {
        private readonly IWorkflowStateFactory _workflowStateFactory;

        public Workflow(IWorkflowStateFactory workflowStateFactory)
        {
            _workflowStateFactory = workflowStateFactory;
        }

        private IWorkflowState CurrentState { get; set; }

        public void Start(IWorkflowOutput workflowOutput)
        {
            CurrentState = _workflowStateFactory.GetPendingState(workflowOutput);
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