using System.IO;
using System.Reflection;
using RepositoryServices.Models;
using WorkflowService.Interfaces;
using WorkflowService.Resources;

namespace WorkflowService.States
{
    public class DisplayMachineDataState : WorkflowState
    {
        private readonly IReadValueService _readValueService;
        private readonly Machine _machine;
        private string BaseDirectory => Path.GetDirectoryName(Assembly.GetEntryAssembly().Location) ?? string.Empty;
        public DisplayMachineDataState(
            IReadValueService readValueService,
            IWorkflowOutput workflowOutput, 
            IWorkflowStateFactory workflowStateFactory, 
            Machine machine) : base(workflowOutput, workflowStateFactory)
        {
            _readValueService = readValueService;
            _machine = machine;
        }

        public override IWorkflowState Initialize()
        {
            WorkflowOutput.ImagePath = Path.Combine(BaseDirectory, _machine.ImageA);
            WorkflowOutput.Description = _machine.Comment;

            if (_machine.EnginePositionA.HasValue)
            {
                WorkflowOutput.Message = StateResources.DisplayOneMessage;
                if (!_readValueService.ReadValue(_machine.EnginePositionA.Value))
                {
                    return WorkflowStateFactory.GetPendingState(WorkflowOutput);
                }
            }

            if (_machine.EnginePositionB.HasValue)
            {
                WorkflowOutput.Message = StateResources.DisplaySecondMessage;
                if (!_readValueService.ReadValue(_machine.EnginePositionB.Value))
                {
                    return WorkflowStateFactory.GetPendingState(WorkflowOutput);
                }
            }

            return base.Initialize();
        }

        public override string Code => "DISPLAY_DATA";

        public override bool CanBreak => false;
    }
}