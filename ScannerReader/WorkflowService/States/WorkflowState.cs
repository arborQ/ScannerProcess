using RepositoryServices;
using WorkflowService.Models;

namespace WorkflowService.States
{
    public abstract class WorkflowState
    {
        protected readonly IWorkflowOutput WorkflowOutput;
        protected ApplicationService ApplicationService;

        protected WorkflowState(IWorkflowOutput workflowOutput)
        {
            WorkflowOutput = workflowOutput;
            ApplicationService = new ApplicationService();
        }

        public abstract string Code { get; }

        public virtual WorkflowState Trigger(string input)
        {
            WorkflowOutput.Message = string.Empty;

            var parts = input.Split('#');
            if (parts.Length == 2)
            {
                return Trigger(new BarCodeModel { InputValue = input, FirstPart = parts[0], SecondPart = parts[1] });
            }
            else
            {
                return Trigger(new BarCodeModel { InputValue = input });
            }
        }

        public virtual WorkflowState Trigger(BarCodeModel inputCode)
        {
            return this;
        }
    }
}