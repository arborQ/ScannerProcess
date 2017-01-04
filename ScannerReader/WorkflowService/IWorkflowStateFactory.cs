using RepositoryServices.Models;
using WorkflowService.States;

namespace WorkflowService
{
    public interface IWorkflowStateFactory
    {
        IWorkflowState GetPendingState(IWorkflowOutput workflowOutput);
        IWorkflowState GetSingleEngineState(IWorkflowOutput workflowOutput, Machine machine);
        IWorkflowState GetMultipleEngineState(IWorkflowOutput workflowOutput, Machine machine);
        IWorkflowState GetDisplayMachineDataState(IWorkflowOutput workflowOutput, Machine machine);
    }
}