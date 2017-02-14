using System.IO;
using RepositoryServices;
using RepositoryServices.Models;
using WorkflowService.Interfaces;
using WorkflowService.Resources;

namespace WorkflowService.States
{
    public class DisplayMachineDataState : WorkflowState
    {
        private readonly IReadValueService _readValueService;
        private readonly ApplicationService _applicationService;
        private readonly Machine _machine;
        private string _baseDirectory;
        private string BaseDirectory => _baseDirectory ?? (_baseDirectory = _applicationService.SettingsRepository.Get().ImagePath);
        public DisplayMachineDataState(
            IReadValueService readValueService,
            IWorkflowOutput workflowOutput, 
            IWorkflowStateFactory workflowStateFactory, 
            ApplicationService applicationService,
            Machine machine) : base(workflowOutput, workflowStateFactory)
        {
            _readValueService = readValueService;
            _applicationService = applicationService;
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