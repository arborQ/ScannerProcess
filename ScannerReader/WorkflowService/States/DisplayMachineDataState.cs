using RepositoryServices.Models;

namespace WorkflowService.States
{
    public class DisplayMachineDataState : WorkflowState
    {
        private readonly Machine _machine;

        public DisplayMachineDataState(IWorkflowOutput workflowOutput, IWorkflowStateFactory workflowStateFactory, Machine machine) : base(workflowOutput, workflowStateFactory)
        {
            _machine = machine;
        }

        public override string Code => "DISPLAY_DATA";
    }
}