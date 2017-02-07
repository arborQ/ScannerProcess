using System.Linq;
using RepositoryServices;
using WorkflowService.Models;
using WorkflowService.Resources;

namespace WorkflowService.States
{
    public class PendingWorklowState : WorkflowState
    {
        private readonly ApplicationService _applicationService;

        public PendingWorklowState(IWorkflowOutput workflowOutput, IWorkflowStateFactory workflowStateFactory, ApplicationService applicationService) : base(workflowOutput, workflowStateFactory)
        {
            _applicationService = applicationService;
        }

        public override string Code => "PENDING";

        public override IWorkflowState Initialize()
        {
            WorkflowOutput.Message = StateResources.PendingInitMessage;
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