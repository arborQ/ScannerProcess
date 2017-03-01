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
        private readonly List<string> _scennedSerialNumbers = new List<string>();
        private const int EngineCount = 2;

        public MultipleEnginePompState(IWorkflowOutput workflowOutput, IWorkflowStateFactory workflowStateFactory, Machine machine) : base(workflowOutput, workflowStateFactory)
        {
            _machine = machine;
        }

        public override string Code => "MULTIPLE_POMP";

        public override IWorkflowState Initialize()
        {
            WorkflowOutput.Description = null;
            WorkflowOutput.ImagePath = null;
            WorkflowOutput.Message = string.Format(StateResources.MultipleEngineInitFormatMessage, EngineCount);
            return base.Initialize();
        }

        protected override IWorkflowState Trigger(BarCodeModel inputCode)
        {
            if (inputCode.SecondPart == _machine.EngineCodeA || inputCode.SecondPart == _machine.EngineCodeB)
            {
                if (_scennedSerialNumbers.Contains(inputCode.FirstPart))
                {
                    WorkflowOutput.Message =
                        $"Nie mo¿na zeskanowaæ drugiego silnika z takim samym numerem seryjnym {inputCode.FirstPart}";

                    return base.Trigger(inputCode);
                }
                else
                {
                    _scennedCodes.Add(inputCode.SecondPart);
                    _scennedSerialNumbers.Add(inputCode.FirstPart);
                }
            }

            WorkflowOutput.Message = string.Format(StateResources.ScanMoreEngineCodesMessageFormat, string.Join(", ", _scennedSerialNumbers.Distinct()));

            if (_scennedSerialNumbers.Distinct().Count() == EngineCount)
            {
                return WorkflowStateFactory.GetDisplayMachineDataState(WorkflowOutput, _machine);
            }

            return base.Trigger(inputCode);
        }
    }
}