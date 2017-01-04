using System.Linq;
using WorkflowService.Models;
using WorkflowService.Resources;

namespace WorkflowService.States
{
    public class PendingWorklowState : WorkflowState
    {
        public PendingWorklowState(IWorkflowOutput workflowOutput, IWorkflowStateFactory workflowStateFactory) : base(workflowOutput, workflowStateFactory)
        {
            WorkflowOutput.Message = StateResources.PendingInitMessage;
        }

        public override string Code => "PENDING";

        protected override IWorkflowState Trigger(BarCodeModel inputCode)
        {
            var record = ApplicationService.MachineRepository.GetRecords(r => r.Code == inputCode.FirstPart).SingleOrDefault();
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