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
            _machine = machine;
        }

        public override string Code => "SINGLE_POMP";

        public override IWorkflowState Initialize()
        {
            WorkflowOutput.Message = StateResources.SingleEngineInitMessage;
            return base.Initialize();
        }

        protected override IWorkflowState Trigger(BarCodeModel inputCode)
        {
            if (_machine.EngineCodeA == inputCode.FirstPart)
            {
                if(_machine.Code != inputCode.SecondPart)
                {
                    WorkflowOutput.Message = $"Kod zamówienia ({_machine.Code}) nie pasuje do kodu z silnika ({ inputCode.SecondPart})";
                    return this;
                }

                return WorkflowStateFactory.GetDisplayMachineDataState(WorkflowOutput, _machine);
            }

            WorkflowOutput.Message = string.Format(StateResources.EngineCodeDoesntMachOrderCodeMessageFormat,
                inputCode.FirstPart, _machine.Code);

            return this;
        }
    }
}