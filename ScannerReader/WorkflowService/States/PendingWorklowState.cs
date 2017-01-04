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
            if (input.Contains("#"))
            {
                WorkflowOutput.Message = "Valid code " + input;
                return base.Trigger(input);
                //return new InitializedWorkflowState(WorkflowOutput);
            }
            WorkflowOutput.Message = "Not valid code";
            return base.Trigger(input);
        }
    }
}