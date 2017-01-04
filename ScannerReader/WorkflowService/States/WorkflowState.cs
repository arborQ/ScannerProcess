namespace WorkflowService.States
{
    public abstract class WorkflowState
    {
        protected readonly IWorkflowOutput WorkflowOutput;

        protected WorkflowState(IWorkflowOutput workflowOutput)
        {
            WorkflowOutput = workflowOutput;
        }

        public abstract string Code { get; }

        public virtual WorkflowState Trigger(string input)
        {
            return this;
        }
    }
}