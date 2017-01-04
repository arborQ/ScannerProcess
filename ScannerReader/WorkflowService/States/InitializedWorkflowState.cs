namespace WorkflowService.States
{
    public class InitializedWorkflowState : WorkflowState
    {
        public InitializedWorkflowState(IWorkflowOutput workflowOutput) : base(workflowOutput)
        {
            WorkflowOutput.Message = Code;
        }

        public override string Code => "INITIALIZED";
    }
}