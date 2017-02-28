using System.Collections.Generic;
using System.Linq;
using RepositoryServices.Models;
using WorkflowService.Models;
using WorkflowService.Resources;

namespace WorkflowService.States
{
    public class MultipleEnginePompState : WorkflowState
    {
        private readonly Machine _machine;
        private readonly List<string> _scennedCodes = new List<string>();
        private const int EngineCount = 2;

        public MultipleEnginePompState(IWorkflowOutput workflowOutput, IWorkflowStateFactory workflowStateFactory, Machine machine) : base(workflowOutput, workflowStateFactory)
        {
            _machine = machine;
        }

        public override string Code => "MULTIPLE_POMP";

        public override IWorkflowState Initialize()
        {
            WorkflowOutput.Message = string.Format(StateResources.MultipleEngineInitFormatMessage, EngineCount);
            return base.Initialize();
        }

        protected override IWorkflowState Trigger(BarCodeModel inputCode)
        {
            if (inputCode.FirstPart == _machine.EngineCodeA || inputCode.FirstPart == _machine.EngineCodeB)
            {
                if (_machine.Code != inputCode.SecondPart)
                {
                    WorkflowOutput.Message = $"Kod zamówienia ({_machine.Code}) nie pasuje do kodu z silnika ({ inputCode.SecondPart})";
                    return this;
                }

                _scennedCodes.Add(inputCode.FirstPart);
            }

            WorkflowOutput.Message = string.Format(StateResources.ScanMoreEngineCodesMessageFormat, string.Join(", ", _scennedCodes.Distinct()));

            if (_scennedCodes.Distinct().Count() == EngineCount)
            {
                return WorkflowStateFactory.GetDisplayMachineDataState(WorkflowOutput, _machine);
            }

            return base.Trigger(inputCode);
        }
    }
}