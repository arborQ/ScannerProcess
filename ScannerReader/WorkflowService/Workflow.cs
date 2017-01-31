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
            CurrentState = _workflowStateFactory.GetPendingState(workflowOutput).Initialize();
        }

        public void Trigger(string input)
        {
            if (input == null)
            {
                throw new Exception("Workflow was not started");
            }

            var nextState = CurrentState.Trigger(input);

            AssignNextState(nextState);
        }

        private void AssignNextState(IWorkflowState nextState)
        {
            while (true)
            {
                if (!ReferenceEquals(nextState, CurrentState))
                {
                    CurrentState = nextState;
                    nextState = CurrentState.Initialize();
                    continue;
                }
                break;
            }
        }

        public bool CanBreak => CurrentState.CanBreak;
    }
}