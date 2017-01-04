using System.Linq;
using WorkflowService.Models;
using WorkflowService.Resources;

namespace WorkflowService.States
{
    public class PendingWorklowState : WorkflowState
    {
        public PendingWorklowState(IWorkflowOutput workflowOutput) : base(workflowOutput)
        {
            WorkflowOutput.Message = StateResources.PendingInitMessage;
        }

        public override string Code => "PENDING";

        public override WorkflowState Trigger(BarCodeModel inputCode)
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
                    return new SingleEnginePompState(WorkflowOutput, record);
                }
                else
                {
                    return new MultipleEnginePompState(WorkflowOutput, record);
                }
            }

            return this;
        }
    }
}