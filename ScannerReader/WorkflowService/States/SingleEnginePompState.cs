using RepositoryServices.Models;
using WorkflowService.Models;
using WorkflowService.Resources;

namespace WorkflowService.States
{
    public class SingleEnginePompState : WorkflowState
    {
        private readonly Machine _machine;

        public SingleEnginePompState(IWorkflowOutput workflowOutput, IWorkflowStateFactory workflowStateFactory, Machine machine) : base(workflowOutput, workflowStateFactory)
        {
            workflowOutput.Message = StateResources.SingleEngineInitMessage;
            _machine = machine;
        }

        public override string Code => "SINGLE_POMP";

        protected override IWorkflowState Trigger(BarCodeModel inputCode)
        {
            if (_machine.EngineCodeA == inputCode.SecondPart)
            {
                return WorkflowStateFactory.GetDisplayMachineDataState(WorkflowOutput, _machine);
            }
            else
            {
                WorkflowOutput.Message = string.Format(StateResources.EngineCodeDoesntMachOrderCodeMessageFormat,
                    inputCode.SecondPart, _machine.Code);
            }

            return this;
        }
    }
}