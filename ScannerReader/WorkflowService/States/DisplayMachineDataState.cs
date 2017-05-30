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
            WorkflowOutput.Description = _machine.Comment;

            if (_machine.EnginePositionA.HasValue)
            {
                var position = _machine.EnginePositionA.HasValue && _machine.EnginePositionB.HasValue ? DialogPosition.Right : DialogPosition.Center;
                WorkflowOutput.ImagePath = GetImagePath(_machine.ImageA);
                WorkflowOutput.Message = StateResources.DisplayOneMessage;
                if (!_readValueService.ReadValue(_machine.EnginePositionA.Value, position))
                {
                    return WorkflowStateFactory.GetPendingState(WorkflowOutput);
                }
            }

            if (_machine.EnginePositionB.HasValue)
            {
                var position = _machine.EnginePositionA.HasValue && _machine.EnginePositionB.HasValue ? DialogPosition.Left : DialogPosition.Center;
                if (!_readValueService.ReadValue(_machine.EnginePositionB.Value, position))
                {
                    return WorkflowStateFactory.GetPendingState(WorkflowOutput);
                }

                WorkflowOutput.ImagePath = GetImagePath(_machine.ImageB);
                WorkflowOutput.Message = StateResources.DisplaySecondMessage;
            }

            WorkflowOutput.ImagePath = GetImagePath(_machine.ImageC);

            return WorkflowStateFactory.GetTriggerWorkerState(WorkflowOutput, _machine);
        }

        private string GetImagePath(string path)
        {
            return string.IsNullOrEmpty(path) ? null : Path.Combine(BaseDirectory, path);
        }

        public override string Code => "DISPLAY_DATA";

        public override bool CanBreak => false;

        public override bool IsLocked => true;
    }
}