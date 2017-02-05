using RepositoryServices;
using WorkflowService.Models;

namespace WorkflowService.States
{
    public interface IWorkflowState
    {
        IWorkflowState Trigger(string input);

        IWorkflowState Initialize();

        bool CanBreak { get; }

        string Code { get; }
    }

    public abstract class WorkflowState : IWorkflowState
    {
        protected readonly IWorkflowOutput WorkflowOutput;
        protected IWorkflowStateFactory WorkflowStateFactory;
        protected ApplicationService ApplicationService;

        protected WorkflowState(IWorkflowOutput workflowOutput, IWorkflowStateFactory workflowStateFactory)
        {
            WorkflowOutput = workflowOutput;
            WorkflowStateFactory = workflowStateFactory;
            ApplicationService = new ApplicationService();
        }

        public abstract string Code { get; }

        public virtual IWorkflowState Trigger(string input)
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

        protected virtual IWorkflowState Trigger(BarCodeModel inputCode)
        {
            return this;
        }

        public virtual IWorkflowState Initialize()
        {
            return this;
        }

        public virtual bool CanBreak => true;
    }
}