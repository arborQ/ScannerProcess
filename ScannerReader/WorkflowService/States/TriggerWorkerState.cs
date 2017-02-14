using System;
using RepositoryServices.Models;
using WorkflowService.Interfaces;

namespace WorkflowService.States
{
    public class TriggerWorkerState : WorkflowState
    {
        private readonly Machine _machine;
        private readonly IReadValueService _readValueService;

        public TriggerWorkerState(IWorkflowOutput workflowOutput, Machine machine, IWorkflowStateFactory workflowStateFactory, IReadValueService readValueService)
            : base(workflowOutput, workflowStateFactory)
        {
            _machine = machine;
            _readValueService = readValueService;
        }

        public override bool CanBreak => false;

        public override string Code => "TRIGGER_WORKER";

        public override IWorkflowState Initialize()
        {
            WorkflowOutput.Message = "DRILLLLLL";
            _readValueService.Wait(TimeSpan.FromSeconds(5));
            
            return WorkflowStateFactory.GetPendingState(WorkflowOutput);
        }
    }
}