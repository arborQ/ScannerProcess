using System.Linq;
using Logger.Interfaces;
using RepositoryServices;
using WorkflowService.Models;
using WorkflowService.Resources;

namespace WorkflowService.States
{
    public class PendingWorklowState : WorkflowState
    {
        private readonly ApplicationService _applicationService;
        private readonly ILogService _logService;

        public PendingWorklowState(IWorkflowOutput workflowOutput, 
            IWorkflowStateFactory workflowStateFactory, 
            ApplicationService applicationService,
            ILogService logService) : base(workflowOutput, workflowStateFactory)
        {
            _applicationService = applicationService;
            _logService = logService;
        }

        public override string Code => "PENDING";

        public override IWorkflowState Initialize()
        {
            WorkflowOutput.Message = StateResources.PendingInitMessage;
            WorkflowOutput.Description = null;
            WorkflowOutput.ImagePath = null;

            return base.Initialize();
        }

        protected override IWorkflowState Trigger(BarCodeModel inputCode)
        {
            var record = _applicationService.MachineRepository.GetRecords(r => r.Code == inputCode.FirstPart).SingleOrDefault();
            if (record == null)
            {
                WorkflowOutput.Message = string.Format(StateResources.UnknownOrderNumberMessageFormat, inputCode.FirstPart);
            }
            else
            {
                if (!string.IsNullOrEmpty(record.EngineCodeA) || !string.IsNullOrEmpty(record.EngineCodeB))
                {
                    return WorkflowStateFactory.GetSingleEngineState(WorkflowOutput, record);
                }
                else
                {
                    return WorkflowStateFactory.GetMultipleEngineState(WorkflowOutput, record);
                }
            }

            return this;
        }
    }
}