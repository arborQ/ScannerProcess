using System;
using System.Threading.Tasks;
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

        public async void Trigger(string input)
        {
            if (input == null)
            {
                throw new Exception("Workflow was not started");
            }

            var nextState = CurrentState.Trigger(input);

           await AssignNextState(nextState);
        }

        private async Task AssignNextState(IWorkflowState nextState)
        {
            while (true)
            {
                if (!ReferenceEquals(nextState, CurrentState))
                {
                    CurrentState = nextState;
                    nextState = CurrentState.Initialize() ?? await CurrentState.AsyncInitialize();
                    continue;
                }
                break;
            }
        }

        public bool CanBreak => CurrentState.CanBreak && !IsLocked;

        public bool IsLocked => CurrentState.IsLocked;
    }
}