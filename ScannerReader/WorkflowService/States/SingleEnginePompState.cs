using RepositoryServices.Models;
using WorkflowService.Models;
using WorkflowService.Resources;

namespace WorkflowService.States
{
    public class SingleEnginePompState : WorkflowState
    {
        private readonly Machine _machine;

        public SingleEnginePompState(IWorkflowOutput workflowOutput, Machine machine) : base(workflowOutput)
        {
            workflowOutput.Message = StateResources.SingleEngineInitMessage;
            _machine = machine;
        }

        public override string Code => "SINGLE_POMP";

        public override WorkflowState Trigger(BarCodeModel inputCode)
        {
            if (_machine.EngineCodeA == inputCode.SecondPart)
            {
                return new DisplayMachineDataState(WorkflowOutput, _machine);
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