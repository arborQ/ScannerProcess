namespace WorkflowService.States
{
    public class PendingWorklowState : WorkflowState
    {
        public PendingWorklowState(IWorkflowOutput workflowOutput) : base(workflowOutput)
        {
            WorkflowOutput.Message = Code;
        }

        public override string Code => "PENDING";

        public override WorkflowState Trigger(string input)
        {
            return new InitializedWorkflowState(WorkflowOutput);
        }
    }

    public class InitializedWorkflowState : WorkflowState
    {
        public InitializedWorkflowState(IWorkflowOutput workflowOutput) : base(workflowOutput)
        {
            WorkflowOutput.Message = Code;
        }

        public override string Code => "INITIALIZED";
    }
}