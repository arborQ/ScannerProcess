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

        public MultipleEnginePompState(IWorkflowOutput workflowOutput, Machine machine) : base(workflowOutput)
        {
            _machine = machine;
            workflowOutput.Message = string.Format(StateResources.MultipleEngineInitFormatMessage, EngineCount);
        }

        public override string Code => "MULTIPLE_POMP";

        public override WorkflowState Trigger(BarCodeModel inputCode)
        {
            if (inputCode.SecondPart == _machine.EngineCodeA || inputCode.SecondPart == _machine.EngineCodeB)
            {
                _scennedCodes.Add(inputCode.SecondPart);
            }

            WorkflowOutput.Message = string.Format(StateResources.ScanMoreEngineCodesMessageFormat, string.Join(", ", _scennedCodes.Distinct()));

            if (_scennedCodes.Distinct().Count() == EngineCount)
            {
                return new DisplayMachineDataState(WorkflowOutput, _machine);
            }

            return base.Trigger(inputCode);
        }
    }
}